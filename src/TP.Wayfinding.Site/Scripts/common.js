$(function () {
    jQuery.ajaxSettings.traditional = true;
    initializeForms();
    initializeLists();
});

(function ($) {
    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=');
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))
})(jQuery);

var loading;
loading = loading || (function () {
    var pleaseWaitDiv = $('<div class="modal" id="loadingModal" tabindex="-1" role="dialog" aria-labelledby="loadingModal" aria-hidden="true"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">Loading...</h4></div></div></div>');
    return {
        show: function () {
            pleaseWaitDiv.modal('show');
        },
        hide: function () {
            pleaseWaitDiv.modal('hide');
        },

    };
})();

if (!window.executeFunctionByName) {
    window.executeFunctionByName = function (functionName, context /*, args */) {
        var args = Array.prototype.slice.call(arguments, 2);
        var namespaces = functionName.split(".");
        var func = namespaces.pop();
        for (var i = 0; i < namespaces.length; i++) {
            context = context[namespaces[i]];
        }
        return context[func].apply(context, args);
    }
}

if (!window.extractErrors) {
    window.extractErrors = function (jqXhr) {
        var data = $.parseJSON(jqXhr.responseText),
        errors = {};

        for (key in data.modelState) {
            var errorKey = key.split('.')[1].camelCase();
            errors[errorKey] = data.modelState[key][0];
        }

        return errors;
    }
}

if (!window.notify) {
    window.notify = function (title, text) {
        $.gritter.add({
            // (string | mandatory) the heading of the notification
            title: title,
            // (string | mandatory) the text inside the notification
            text: text,
            time: ''
        });
    }
}

if (!window.notifyUpdate) {
    window.notifyUpdate = function (title, text) {
        window.notify('Update Successful!', '');
    };
}
    
if (!window.notifyCreate) {
    window.notifyCreate = function (title, text) {
        window.notify('Create Successful!', '');
    };
}

if (!window.notifyDelete) {
    window.notifyDelete = function (title, text) {
        window.notify('Delete Successful!', '');
    };
}

String.prototype.camelCase = function () {
    return this
        .replace(/\s(.)/g, function ($1) { return $1.toUpperCase(); })
        .replace(/\s/g, '')
        .replace(/^(.)/, function ($1) { return $1.toLowerCase(); });
    //return this.toLowerCase().replace(/-(.)/g, function (match, group1) {
    //    return group1.toUpperCase();
    //});
};

if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    };
}

var isFunction = function (functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
};

var isEmpty = function (objectToCheck) {
    if (objectToCheck == null) return true;
    if (objectToCheck.length > 0) return false;
    if (objectToCheck.length === 0) return true;

    for (var key in objectToCheck) {
        if (hasOwnProperty.call(objectToCheck, key)) return false;
    }

    return true;
};

var initializeForms = function () {
    $('form.form-validate').each(function () {
        if (typeof ($.validator) == "undefined") {
            return;
        }
        if (typeof ($(this).data("validator")) == "undefined") {
            $.validator.unobtrusive.parse(this);
        }
        if (typeof ($(this).data("validator")) == "undefined") {
            return;
        }
        var settings = $(this).data("validator").settings;
        settings.ignore = ".ignore";
        settings.errorClass = "error";
        settings.errorElement = 'span';
        //settings.showErrors = function (errorMap, errorList) {
        //    this.defaultShowErrors();
        //    //showErrorsStyled();
        //};
    });
    //showErrorsStyled();
};

var initializeLists = function () {
};

var showErrorsStyled = function () {
    $('.valid').removeClass('error');

    $('.input-validation-error').addClass('error');

    $('.field-validation-error').each(function () {
        var self = $(this);

        var parent = $(this).parents("div:first");
        var errorLabel = parent.find("label.error");
        errorLabel.text('');
        self.detach().appendTo(errorLabel);
    });
};


var submitDynamicForm = function (url, params) {
    if (method == null || typeof (method) == "undefined") {
        method = "POST";
    }
    var form = ['<form method="POST" action="', url, '">'];
    for (var key in params) {
        form.push('<input type="hidden" name="', key, '" value="', params[key], '"/>');
    }

    form.push('</form>');
    jQuery(form.join('')).appendTo('body')[0].submit();
};
