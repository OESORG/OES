(function ($) {
    $.fn.deleteButton = function () {
        $(this).each(function () {
            var $this = $(this);
            var confim = document.createElement("div");
            confim.innerHTML = "Are you sure? <a><i class='fa fa-check'></i></a> <a><i class='fa fa-close' ></i></a>";
            var $confirm = $(confim).hide();
            $this.after($confirm);
            $this.click(function () {
                $this.fadeOut(500);
                $confirm.fadeIn(200);
                return false;
            })
        })
    }
})(jQuery)