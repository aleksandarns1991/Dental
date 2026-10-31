using Dental.Models;
using Dental.Services;
using Splat;

namespace Dental.Utility
{
    public class AppBootStrapper
    {
        public AppBootStrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new DataPersistence<Doctor>(),typeof(IDataPersistence<Doctor>));
            Locator.CurrentMutable.RegisterConstant(new DataPersistence<Patient>(),typeof(IDataPersistence<Patient>));
            Locator.CurrentMutable.RegisterConstant(new DataPersistence<Visit>(),typeof(IDataPersistence<Visit>));
            Locator.CurrentMutable.RegisterConstant(new DataPersistence<Intervention>(), typeof(IDataPersistence<Intervention>));
        }
    }
}
