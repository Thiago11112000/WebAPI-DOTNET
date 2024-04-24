using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using WebAPI_Video.DataContext;
using WebAPI_Video.Models;
using System.Reflection;

namespace WebAPI_Video.Service.FuncionarioService
{
    public class FuncionarioService : IFuncionarioInterface
    {
        private readonly ApplicationDbContext _context;
        public FuncionarioService(ApplicationDbContext context)
        {
            _context = context;
        }





        public async Task<ServiceResponse<List<FuncionarioModel>>> CreateFuncionario(FuncionarioModel novoFuncionario)
        {
            ServiceResponse<List<FuncionarioModel>> serviceResponse = new ServiceResponse<List<FuncionarioModel>>();

            List<string> missingProperties = new List<string>();

            if (novoFuncionario == null)
            {
                missingProperties.Add("FuncionarioModel");
            }
            else
            {
                PropertyInfo[] properties = typeof(FuncionarioModel).GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(novoFuncionario);

                    if (property.PropertyType == typeof(string))
                    {
                        if (string.IsNullOrEmpty((string)value))
                        {
                            missingProperties.Add(property.Name);
                        }
                    }
                    else if (property.PropertyType == typeof(int?) && value == null)
                    {
                        missingProperties.Add(property.Name);
                    }
                    else if (property.PropertyType == typeof(bool?) && value == null)
                    {
                        missingProperties.Add(property.Name);
                    }
                }
            }

            if (missingProperties.Any())
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = $"Informar os seguintes dados: {string.Join(", ", missingProperties)}";
                serviceResponse.Sucesso = false;

                return serviceResponse;
            }

            try
            {
                novoFuncionario.DataDeCriacao = DateTime.Now.ToLocalTime();
                novoFuncionario.DataDeAlteracao = DateTime.Now.ToLocalTime();

                _context.Add(novoFuncionario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Funcionarios.ToList();
                serviceResponse.Sucesso = true;
                serviceResponse.Mensagem = "Funcionário criado com sucesso!";
            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Sucesso = false;
                serviceResponse.Mensagem = $"Erro ao criar funcionário: {ex.Message}";
            }

            return serviceResponse;
        }



        public async Task<ServiceResponse<List<FuncionarioModel>>> DeleteFuncionario(int id)
        {
            ServiceResponse<List<FuncionarioModel>> serviceResponse = new ServiceResponse<List<FuncionarioModel>>();

            try
            {
                FuncionarioModel funcionario = _context.Funcionarios.FirstOrDefault(x => x.Id == id);

                if (funcionario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }


                _context.Funcionarios.Remove(funcionario);
                await _context.SaveChangesAsync();


                serviceResponse.Dados = _context.Funcionarios.ToList();

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }




        public async Task<ServiceResponse<FuncionarioModel>> GetFuncionarioById(int id)
        {
            ServiceResponse<FuncionarioModel> serviceResponse = new ServiceResponse<FuncionarioModel>();

            try
            {
                FuncionarioModel funcionario = _context.Funcionarios.FirstOrDefault(x => x.Id == id);
                if (funcionario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;
                }
                serviceResponse.Dados = funcionario;

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }




        public async Task<ServiceResponse<List<FuncionarioModel>>> GetFuncionarios()
        {
            ServiceResponse<List<FuncionarioModel>> serviceResponse = new ServiceResponse<List<FuncionarioModel>>();

            try
            {
                serviceResponse.Dados = _context.Funcionarios.ToList();
                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FuncionarioModel>>> InativaFuncionario(int id)
        {
            ServiceResponse<List<FuncionarioModel>> serviceResponse = new ServiceResponse<List<FuncionarioModel>>();

            try
            {
                FuncionarioModel funcionario = _context.Funcionarios.FirstOrDefault(x => x.Id == id);

                if (funcionario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;
                }

                funcionario.Ativo = false;
                funcionario.DataDeAlteracao = DateTime.Now.ToLocalTime();

                _context.Funcionarios.Update(funcionario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Funcionarios.ToList();


            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<List<FuncionarioModel>>> UpdateFuncionario(FuncionarioModel editadoFuncionario)
        {
            ServiceResponse<List<FuncionarioModel>> serviceResponse = new ServiceResponse<List<FuncionarioModel>>();

            try
            {
                FuncionarioModel funcionario = _context.Funcionarios.AsNoTracking().FirstOrDefault(x => x.Id == editadoFuncionario.Id);

                if (funcionario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;
                }


                funcionario.DataDeAlteracao = DateTime.Now.ToLocalTime();

                _context.Funcionarios.Update(editadoFuncionario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Funcionarios.ToList();

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
    }
}

