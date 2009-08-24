/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

$("#shippingMethod").change(function() {
	$(this).parents("form").submit();
});

$(".delete-department-button").click(function() {
	var departmentId = $(this).attr("meta:id");
	$.post(
		"/Commerce/DeleteDepartment",
		{ id: departmentId },
		function(data) {
			$("#department-" + data.object.id).remove();
			$("#admin-" + data.object.id).remove();
			$("#spacer-" + data.object.id).remove();
		},
		"json"
	);
	return false;
});

$(".delete-product-button").click(function() {
	var productId = $(this).attr("meta:id");
	$.post(
		"/Commerce/DeleteProduct",
		{ id: productId },
		function(data) {
			$("#product-" + data.object.id).remove();
			$("#admin-" + data.object.id).remove();
			$("#spacer-" + data.object.id).remove();
		},
		"json"
	);
	return false;
});

$(".delete-shipping-method-button").live("click", function() {
	var shippingMethodId = $(this).attr("meta:id");
	$.post(
		"/Commerce/DeleteShipping",
		{ id: shippingMethodId },
		function(data) {
			$("#shipping-method-" + data.object.id).remove();
		},
		"json"
	);
	return false;
});

$("#add-shipping-method-button").click(function() {
	var title = $("#title").val();
	var price = $("#price").val();

	$.post(
		"/Commerce/CreateShipping",
		{ title: title, price: price },
		function(data) {
			var html = '<tr id="shipping-method-' + data.object.id + '">';
			html += '<td align="center">' + title + '</td>';
			html += '<td align="center">' + price + '</td>';
			html += '<td align="center"><a href="#" class="delete-shipping-method-button" meta:id="' + data.object.id + '"><img border="0" alt="Delete Role" src="/content/images/DeleteSymbol.png" title="Delete Role" align="middle"/></a></td>';
			html += '</tr>';

			$("#shipping-table tbody").append(html);
			$("#title").val("");
			$("#price").val("");
		},
		"json"
	);

	return false;
});

$(".delete-item-button").click(function() {
	var productId = $(this).attr("meta:id");
	$.post(
		"/Commerce/DeleteShoppingCartItem",
		{ id: productId },
		function(data) {
			$("#item-" + data.object.id).remove();
		},
		"json"
	);
	return false;
});