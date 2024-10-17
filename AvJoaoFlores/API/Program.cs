using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "API Rodando na Porta 5043!");

app.MapGet("/api/funcionario/listar", ([FromServices] AppDataContext ctx) => 
{
  if (ctx.Funcionarios.Any())
  {
    return Results.Ok(ctx.Funcionarios.ToList());
  }
  return Results.NoContent();
});

app.MapPost("/api/funcionario/cadastrar", ([FromBody] Funcionario funcionario, [FromServices] AppDataContext ctx) => 
{
  ctx.Funcionarios.Add(funcionario);
  ctx.SaveChanges();
  return Results.Created("Funcionario Cadastrado!", ctx.Funcionarios.ToList());
});

app.MapGet("/api/folha/listar", ([FromServices] AppDataContext ctx) => 
{
  if (ctx.Folhas.Any())
  {
    return Results.Ok(ctx.Folhas.ToList());
  }
  return Results.NoContent();
});

app.MapPost("/api/folha/cadastrar", ([FromBody] Folha folha, [FromServices] AppDataContext ctx) => 
{
  Folha? newFolha = folha;
  float valorH = newFolha.Valor;
  int quantH = newFolha.Quantidade;
  int mes = newFolha.Mes;
  int ano = newFolha.Ano;
  //____________________________________________________________________________
  newFolha.SalarioBruto = valorH * quantH;
  float salBrut = newFolha.SalarioBruto;
  //____________________________________________________________________________
  if (salBrut <= 1903.98f)
  {
    newFolha.ImpostoIrrf = newFolha.SalarioBruto;
  }
  else if (salBrut <= 2826.65f )
  {
    newFolha.ImpostoIrrf = newFolha.SalarioBruto * 7.5f / 100;
  }
  else if (salBrut <= 3751.05f)
  {
    newFolha.ImpostoIrrf = newFolha.SalarioBruto * 15 / 100;
  }
  else if (salBrut <= 4664.68f)
  {
    newFolha.ImpostoIrrf = newFolha.SalarioBruto * 22.5f / 100;
  }
  else
  {
    newFolha.ImpostoIrrf = newFolha.SalarioBruto * 27.5f / 100;
  }
  float ImpIrrf = newFolha.ImpostoIrrf;
  //____________________________________________________________________________
  if (salBrut <= 1693.72f)
  {
    newFolha.ImpostoInss = newFolha.SalarioBruto * 8 / 100;
  }
  else if (salBrut <= 2822.90f)
  {
    newFolha.ImpostoInss = newFolha.SalarioBruto * 9 / 100;
  }
  else if (salBrut <= 5645.80f)
  {
    newFolha.ImpostoInss = newFolha.SalarioBruto * 11 / 100;
  }
  else
  {
    newFolha.ImpostoInss = 621.03f;
  }
  float ImpInss = newFolha.ImpostoInss;
  //____________________________________________________________________________
  newFolha.ImpostoFgts = newFolha.SalarioBruto - (newFolha.SalarioBruto * 8 / 100);
  //____________________________________________________________________________
  newFolha.SalarioLiquido = newFolha.SalarioBruto - ImpIrrf - ImpInss;
  //____________________________________________________________________________
  ctx.Folhas.Add(newFolha);
  ctx.SaveChanges();
  return Results.Ok(ctx.Folhas.ToList());
});

app.MapGet("/api/folha/buscar/{cpf}/{mes}/{ano}", ([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano, [FromServices] AppDataContext ctx) => 
{
  Funcionario? funcionario = ctx.Funcionarios.Find(cpf);
  if (funcionario != null)
  {
    int funcId = funcionario.FuncionarioId;
  
    Folha? folha = ctx.Folhas.FirstOrDefault(x => x.FuncionarioId == funcId && x.Mes == mes && x.Ano == ano);
    if (folha != null)
    {
      return Results.Ok(folha);
    }
    return Results.NotFound();
  }
  return Results.NotFound();
});

// public static void SalarioBruto()
// {}

app.Run();
