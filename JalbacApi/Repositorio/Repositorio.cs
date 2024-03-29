﻿using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace JalbacApi.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly BdJalbacContext _db;
        internal DbSet<T> dbSet;
        public Repositorio(BdJalbacContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Crear(T obj)
        {
            await dbSet.AddAsync(obj);
            await Grabar();
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Editar(T obj)
        {
            dbSet.Update(obj);
            await Grabar();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, bool tracked = true, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                    if (incluirProp == "IdUsuarioNavigation")
                    {
                        query = query.Include("IdUsuarioNavigation.IdRolNavigation");
                    }
                    if (incluirProp == "IdPedidoNavigation")
                    {
                        query = query.Include("IdPedidoNavigation.IdClienteNavigation");
                    }
                }
            }

            return await query.ToListAsync();
        }

        public async Task Remover(T obj)
        {
            dbSet.Remove(obj);
            await Grabar();
        }

        public async Task<List<T>> Consultar(Expression<Func<T, bool>> filtro = null)
        {
            try
            {
                IQueryable<T> queryModelo = filtro == null ? _db.Set<T>() :
                    _db.Set<T>().Where(filtro);
                return await queryModelo.ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
