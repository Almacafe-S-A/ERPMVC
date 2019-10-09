using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class NodeViewModel
    {
        public NodeViewModel()
        {
            this.Expanded = true;
            this.Children = new List<NodeViewModel>();
        }

        public Int64 Id { get; set; }
        public string Title { get; set; }
        public double Balance { get; set; }

        public bool Expanded { get; set; }

        public bool HasChildren
        {
            get { return Children.Any(); }
        }

        public IList<NodeViewModel> Children { get; private set; }

    }
}
