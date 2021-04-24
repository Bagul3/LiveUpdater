using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveUpdate
{
    public class LiveStockRepo
    {
        public HDF DoesHDFExist(string hdf)
        {
            using(var db = new LiveStockEntities())
            {
                var result = db.HDFs.FirstOrDefault(x => x.hdf_name.Equals(hdf));
                return result;
            }
        }

        public void Insert(HDF hdf)
        {
            using (var db = new LiveStockEntities())
            {
                db.HDFs.Add(hdf);
                db.SaveChanges();
            }
        }
    }
}
