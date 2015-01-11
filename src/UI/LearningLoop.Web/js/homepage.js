$(document)
  .ready(function () {
      $('.standard.auth.modal').modal('attach events', '.item.auth', 'show');
      $('.ui.checkbox').checkbox();
      $('.ui.dropdown').dropdown({
          action: 'hide'
      });
     
      $('.ui.sticky').sticky();
  });