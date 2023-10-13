var lpTargetSegmentCount = 0;
var lpDistributionChannelCount = 0;
var lpUnderwritingMethodCount = 0;

$(document).ready(function () {
    initializeTokenField('#lpTargetSegments', TargetSegmentCodes, 'lpTargetSegmentCount');
    initializeTokenField('#lpDistributionChannels', DistributionChannelCodes, 'lpDistributionChannelCount');
    initializeTokenField('#lpUnderwritingMethods', UnderwritingMethodCodes, 'lpUnderwritingMethodCount');   

    if (ReadOnly) {
        $('#linkProductModalBtn').prop('disabled', true);
    }
});

function openLinkProductModal() {
    $('#lpTreatyPricingCedantId').val(DefaultTreatyPricingCedantId);
    $('#lpProductTypeId').val(null);
    $('#lpTargetSegments').tokenfield('setTokens', []);
    $('#lpDistributionChannels').tokenfield('setTokens', []);
    $('#lpUnderwritingMethods').tokenfield('setTokens', []);
    $('#lpProductIdName').val(null);
    $('#lpQuotationName').val(null);

    showNoLinkProductFound();
    resetSelectedLinkProducts();
    resetLinkProductError();
}

function searchProduct() {
    resetSelectedLinkProducts();
    resetLinkProductError();

    var params = {
        ParentId: Model.Id,
        TreatyPricingCedantId: $('#lpTreatyPricingCedantId').val(),
        ProductTypeId: $('#lpProductTypeId').val(),
        TargetSegments: $('#lpTargetSegments').val(),
        DistributionChannels: $('#lpDistributionChannels').val(),
        UnderwritingMethods: $('#lpUnderwritingMethods').val(),
        ProductIdName: $('#lpProductIdName').val(),
        QuotationName: $('#lpQuotationName').val(),
    };

    var products = [];
    $.ajax({
        url: SearchProductUrl,
        type: "POST",
        data: params,
        cache: false,
        async: false,
        success: function (data) {
            products = data.bos;
        }
    });

    populateLinkProductTable(products);
}

function linkProducts() {
    resetLinkProductError();

    var selectedIds = [];
    $('input[name=chkLinkProduct]').each(function () {
        var id = $(this).attr('value');
        if (typeof id != 'undefined') {
            var checked = $(this).prop('checked');
            if (checked)
                selectedIds.push(id);
        }
    });

    var params = {
        ParentId: Model.Id,
        TreatyPricingProductIds: selectedIds
    };

    var isSuccess = false;
    var perLifeRetroProducts = [];
    var errors = [];
    $.ajax({
        url: LinkProductUrl,
        type: "POST",
        data: params,
        cache: false,
        async: false,
        success: function (data) {
            if (data.errors && data.errors.length > 0) {
                errors = data.errors;
            } else {
                isSuccess = true;
                perLifeRetroProducts = data.bos;
            }
        }
    });

    if (isSuccess && perLifeRetroProducts) {
        app[VueProductListName] = perLifeRetroProducts;
        $('#linkProductModal').modal('hide');
    } else if (errors.length > 0) {
        $('#linkProductError').append(arrayToUnorderedList(errors));
        $('#linkProductError').show();
    }
}

function unlinkProduct(index) {
    if (ReadOnly)
        return;

    var product = app[VueProductListName][index];

    if (product == null)
        return;

    var params = {
        ParentId: Model.Id,
        ProductId: product.TreatyPricingProductId
    };

    var perLifeRetroProducts = [];
    $.ajax({
        url: UnlinkProductUrl,
        type: "POST",
        data: params,
        cache: false,
        async: false,
        success: function (data) {
            perLifeRetroProducts = data.bos;
        }
    });

    if (perLifeRetroProducts) {
        app[VueProductListName] = perLifeRetroProducts;
    } 
}

function editProduct(index) {
    var product = app[VueProductListName][index];

    if (product == null)
        return;

    window.open(
        EditProductUrl + '/' + product.TreatyPricingProductId,
        '_blank'
    );
}

function populateLinkProductTable(products) {
    $("#linkProductTable tbody tr").remove();

    if (products == null || products.length == 0) {
        showNoLinkProductFound();
        return;
    }

    $('#linkProductBtn').prop('disabled', false);
    products.forEach(function (product) {
        var productVersion = product.CurrentVersionObject;

        var productType = '';
        if (productVersion.ProductTypePickListDetailBo != null)
            productType = productVersion.ProductTypePickListDetailBo.Description;

        var productName = product.Name != null ? product.Name : '';
        var productSummary = product.Summary != null ? product.Summary : '';
        var productQuotationName = product.QuotationName != null ? product.QuotationName : '';

        var row = '<tr>';
        if (!HideCedingCompany) {
            row += '<td>' + product.TreatyPricingCedantCode + '</td>';
        }
        row += '<td>' + productType + '</td>';
        row += '<td>' + productVersion.TargetSegment + '</td>';
        row += '<td>' + productVersion.DistributionChannel + '</td>';
        row += '<td>' + product.UnderwritingMethod + '</td>';
        row += '<td>' + product.Code + '</td>';
        row += '<td>' + productName + '</td>';
        row += '<td>' + productSummary + '</td>';
        row += '<td>' + productQuotationName + '</td>';
        row += '<td class="text-center"> <input type="checkbox" name="chkLinkProduct" value="' + product.Id + '"></td>';
        row += '</tr>';

        $("#linkProductTable tbody").append(row);
    });
}

function showNoLinkProductFound() {
    $('#linkProductBtn').prop('disabled', true);

    $("#linkProductTable tbody tr").remove();
    var row = '<tr><td colspan="' + ColSpan + '" class="no-data">No available data found.</td></tr>';
    $("#linkProductTable tbody").append(row);
}

function resetSelectedLinkProducts() {
    $('#chkAllLinkProduct').prop('checked', false);
    selectAllLinkProduct(false);
}

function selectAllLinkProduct(checked) {
    $('input[name=chkLinkProduct]').prop('checked', checked);
}

function resetLinkProductError() {
    $('#linkProductError').empty();
    $('#linkProductError').hide();
}

