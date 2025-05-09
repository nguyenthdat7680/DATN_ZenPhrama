using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.InventoryBatchDtos
{
    public class InventoryBatchUpdateDto : InventoryBatchCreateDto
    {
        public Guid Id { get; set; }
    }
}
