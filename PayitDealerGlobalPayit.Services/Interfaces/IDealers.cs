using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayitDealerGlobalPayit.Models;


namespace PayitDealerGlobalPayit.Services.Interfaces
{
    public interface IDealers
    {
        Models.DTO<Models.Outputs.LoginDTO> login(Models.Input<Models.Inputs.LoginIN> obj);

        Models.DTO<Models.Outputs.ServicesDTO> GetServicesByCountry(Models.Input<Models.Inputs.ServicesIN> obj);

        Models.DTO<Models.Outputs.CategoryServices> GetServicesByCountry2(Models.Input<Models.Inputs.ServicesIN> obj);
        Models.DTO<Models.Outputs.ServiceDetailsDTO2> GetServicesDetails(Models.Inputs.ServiceDetailsIN obj);

        
        Models.DTO<Models.Outputs.CategoryServices> GetAllServices(Models.Input<Models.Inputs.GetActiveServices> obj);

        Models.DTO<Models.Outputs.ProcessDTO> Process(Models.Input<Models.Inputs.ProcessIN> obj);

        Models.DTO<Models.Outputs.ProcessDTO> Process2(Models.Input<Models.Inputs.ProcessIN2> obj);

        Models.DTO<Models.Outputs.ServiceAmountsDTO> GetServiceAmounts(Models.Input<Models.Inputs.LoginIN> obj);

        bool checkLogin(string username, string password);
        
    }
}
