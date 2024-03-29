﻿namespace SoftUni
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Models;
    using System.Globalization;
    using System.Text;
    using System.Data;


    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContxt = new SoftUniContext();


            Console.WriteLine(RemoveTown(dbContxt));
        }

        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
                .Where(t => t.Name == "Seattle")
                .FirstOrDefault();

            Address[] addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete.TownId)
                .ToArray();

            Employee[] employeesToRemoveAddressFrom = context.Employees
                .Where(e => addressesToDelete
                .Contains(e.Address))
                .ToArray();

            foreach (Employee e in employeesToRemoveAddressFrom)
            {
                e.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDelete);
            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{addressesToDelete.Count()} addresses in Seattle were deleted";
        }
    }
}