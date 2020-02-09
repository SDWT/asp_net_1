Cart =
{
    _properties:
    {
        getCartViewLink: "",
        addToCartLink: "",
        decrementFromCartLink: "",
        removeFromCartLink: "",
        removeAllLink: "",
        productsCountLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);

        Cart.initEvents();
    },

    initEvents: function () {
        $(".marker-add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_up").click(Cart.incrementItem);
        $(".cart_quantity_down").click(Cart.decrementItem);
        $(".cart_quantity_delete").click(Cart.removeFromCart);
    },

    addToCart: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");

        //$.get(Cart._properties.addToCartLink + "/" + id)
        $.get(`${Cart._properties.addToCartLink}/${id}`)
            .done(function () {
                Cart.showToolTip(button);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("addToCart fail"); });
    },

    showToolTip: function (button) {
        button.tooltip({ title: "Добавлено в корзину" }).tooltip("show");

        setTimeout(function () { button.tooltip("destroy"); }, 500);
    },

    refreshCartView: function () {
        //var container = $("#cart-notification-container");
        //$.get(Cart._properties.getCartViewLink)
        //    .done(function (cart_html) { container.html(cart_html); })
        //    .fail(function () { console.log("refreshCartView fail"); });

        var container2 = $("#cart-count-container");
        var count = parseInt($("#cart-count-container").data("count"));
        $.get(Cart._properties.productsCountLink)
            .done(function (count_json) {
                var count = parseInt(count_json["count"]);
                $("#cart-count-container").data("count", count);
                //console.log(count);
                $("#cart-count-container").html(count);
            })
            .fail(function () { console.log("refreshCartView fail"); });
    },

    incrementItem: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");
        var container = button.closest("tr");

        $.get(`${Cart._properties.addToCartLink}/${id}`)
            .done(function () {
                var count = parseInt($(".cart_quantity_input", container).val());
                $(".cart_quantity_input", container).val(count + 1);

                Cart.refreshPrice(container);
                Cart.refreshCartView();
            })
            .fail(function () { console.log("incrementItem fail"); });
    },

    decrementItem: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");
        var container = button.closest("tr");

        $.get(`${Cart._properties.decrementFromCartLink}/${id}`)
            .done(function () {
                var count = parseInt($(".cart_quantity_input", container).val());
                if (count > 1) {
                    $(".cart_quantity_input", container).val(count - 1);
                    Cart.refreshPrice(container);
                }
                else {
                    container.remove();
                    Cart.refreshTotalPrice();
                }
                Cart.refreshCartView();
            })
            .fail(function () { console.log("decrementItem fail"); });
    },

    removeFromCart: function (event) {
        event.preventDefault();

        var button = $(this);
        var id = button.data("id");

        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done(function () {
                button.closest("tr").remove();
                Cart.refreshTotalPrice();
                Cart.refreshCartView();
            })
            .fail(function () { console.log("removeFromCart fail"); });
    },

    refreshPrice: function (container) {
        var quantity = parseInt($(".cart_quantity_input", container).val());
        var price = parseFloat($(".cart_price", container).data("price"));
        var totalPrice = quantity * price;

        var value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $(".cart_total_price", container).data("price", totalPrice);
        $(".cart_total_price", container).html(value);

        Cart.refreshTotalPrice();
    },

    refreshTotalPrice: function () {
        var total = 0;

        $(".cart_total_price").each(function () {
            var price = parseFloat($(this).data("price"));
            total += price;
        });

        var value = total/*.toPrecision(2)*/.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $("#total-order-sum").html(value);
    }
}
