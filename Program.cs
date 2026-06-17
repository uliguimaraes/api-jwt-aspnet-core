using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configurações JWT
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

// Chave de assinatura
var chave = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtKey!)
);

// Configuração da autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = chave,

            ClockSkew = TimeSpan.Zero
        };
    });

// Configuração das autorizações
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SomenteProfessor", policy =>
    {
        policy.RequireRole("Professor");
    });
});

var app = builder.Build();

// Middlewares
app.UseAuthentication();
app.UseAuthorization();


// =========================
// ROTAS PÚBLICAS
// =========================

app.MapGet("/", () =>
{
    return Results.Ok("API JWT funcionando!");
});

app.MapGet("/publico", () =>
{
    return Results.Ok(
        "Este endpoint é público e pode ser acessado por qualquer pessoa."
    );
});


// =========================
// LOGIN
// =========================

app.MapPost("/login", (LoginRequest login) =>
{
    if (login.Username != "professor" ||
        login.Password != "123456")
    {
        return Results.Unauthorized();
    }

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, login.Username),
        new Claim(ClaimTypes.Role, "Professor")
    };

    var credenciais = new SigningCredentials(
        chave,
        SecurityAlgorithms.HmacSha256
    );

    var token = new JwtSecurityToken(
        issuer: jwtIssuer,
        audience: jwtAudience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credenciais
    );

    var tokenString = new JwtSecurityTokenHandler()
        .WriteToken(token);

    return Results.Ok(new
    {
        Token = tokenString
    });
});


// =========================
// ROTAS PROTEGIDAS
// =========================

app.MapGet("/protegido", () =>
{
    return Results.Ok(
        "Você acessou uma rota protegida!"
    );
})
.RequireAuthorization();

app.MapGet("/professor/aulas", () =>
{
    return Results.Ok(
        "Lista de aulas do professor"
    );
})
.RequireAuthorization("SomenteProfessor");


// =========================
// INICIALIZAÇÃO
// =========================

app.Run();


// =========================
// RECORDS
// =========================

public record LoginRequest(
    string Username,
    string Password
);