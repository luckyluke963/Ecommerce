var user = {
    init: function () {

        user.loadProvince();
      
    },
  
    loadProvince: function () {
        var html = '';
        $.ajax({
            url: '/Account/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    alert('1');
                }
            }
        })
    },
  
}
user.init();