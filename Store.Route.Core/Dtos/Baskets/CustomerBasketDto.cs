﻿using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Dtos.Baskets
{
	public class CustomerBasketDto
	{
		public string Id { get; set; }
		public List<BasketItem> Items { get; set; }
	}
}
