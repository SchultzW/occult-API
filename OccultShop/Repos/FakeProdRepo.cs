using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Midterm.Models;

namespace Midterm.Repos
{
    public class FakeProdRepo : IProdRepos
    {
        public IQueryable<Product> Products => throw new NotImplementedException();
        private List<Product> products = new List<Product>();
        

//        IQueryable<Product> IProdRepos.Products => throw new NotImplementedException();

        public void AddProd(Product p)
        {
            products.Add(p);
        }

        public void AddReview(Product p, Review r)
        {
            p.Reviews.Add(r);
            
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return products.Count();
        }

        public Product GetProdByID(int id)
        {
            Product prod=Products.First(p => p.ProductId == id);
            return prod;
        }

        public Product GetProdByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProd(int id, Product newP)
        {
            throw new NotImplementedException();
        }

       

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }
    }

}
