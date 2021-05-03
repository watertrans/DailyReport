export default {
  
  install: (app) => {

    app.config.globalProperties.convertToSortReuqest = function(value) {
      var result = value.sortField;
      if (value.sortOrder < 0) {
        result = '-' + result;
      }
      return result;
    };

  }
};