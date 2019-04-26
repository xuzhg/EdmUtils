using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdmLib
{
    public class KeModel
    {
        public IList<KeSchema> Schemas { get; set; } = new List<KeSchema>();

        public IList<KeModel> ReferencedModels { get; set; } = new List<KeModel>();

        public void Save(string filePath)
        {

        }
    }

    public static class KeModelHelper
    {
        private static KeModel _model;

        public static KeModel GetSampleModel()
        {
            if (_model != null)
            {
                return _model;
            }

            _model = new KeModel();

            _model.Schemas.Add(new KeSchema { Namespace = "Microsoft.Graph" });
            _model.Schemas.Add(new KeSchema { Namespace = "Test.Namespace" });

            return _model;
        }
    }
}
