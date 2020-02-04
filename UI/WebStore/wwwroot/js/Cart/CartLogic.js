Cart =
{
    _properties:
    {
        getCartViewLink: "",
        addToCartLink: "",
        productsCountLink: ""
    },

    init: function (properties)
    {
        $.extend(Cart._properties, properties);

        Cart.initEvents();
    },

    initEvents: function ()
    {
        $(".marker-add-to-cart").click(Cart.addToCart);
    },

    addToCart: function (event)
    {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");

        //$.get(Cart._properties.addToCartLink + "/" + id)
        $.get(`${Cart._properties.addToCartLink}/${id}`)
            .done(function ()
            {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("addToCart fail"); });
    },

    showToolTip: function (button)
    {
        button.tooltip({ title: "��������� � �������" }).tooltip("show");

        setTimeout(function () { button.tooltip("destroy"); }, 500);
    },

    refreshCartView: function ()
    {
        var container = $("#cart-notification-container");
        $.get(Cart._properties.getCartViewLink)
            .done(function (cart_html) { container.html(cart_html); })
            .fail(function () { console.log("refreshCartView fail"); });

        //var container2 = $("#cart-count-container");
        //$.get(Cart._properties.productsCountLink)
        //    .done(function (count_json) { container2.val(count_json["count"]); console.log(count_json["count"]); })
        //    .fail(function () { console.log("refreshCartView fail"); });
    }
}
