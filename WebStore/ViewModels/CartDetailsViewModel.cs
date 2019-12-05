﻿namespace WebStore.ViewModels
{
    /// <summary>
    /// Полная модель-представления для представления корзины,
    /// где есть область корзины и область оформления заказа
    /// </summary>
    public class CartDetailsViewModel
    {
        public CartViewModel CartViewModel { get; set; }

        public OrderViewModel OrderViewModel { get; set; }
    }
}
