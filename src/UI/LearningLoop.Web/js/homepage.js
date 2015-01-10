$(document)
  .ready(function () {
      $('.standard.auth.modal').modal('attach events', '.item.auth', 'show');
      $('.ui.checkbox').checkbox();
      $('.ui.dropdown').dropdown({
          action: 'hide'
      });
      $('#class-roster.cards .image.dimmable').dimmer({
          on: 'hover'
      });
      $('.ui.sticky').sticky();
  });