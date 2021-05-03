<script>
export default {
  methods: {
    handleUnauthorizedError() {
      this.$confirm.require({
        message: this.$i18n.t('dialog.unauthorizedMessage'),
        header: this.$i18n.t('dialog.unauthorized'),
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          window.location = process.env.VUE_APP_AUTH_BASE_URL + '/Account/Login';
        }
      });
    },
    handleError(error, customMessages = {}) {
      const errorResponse = {
        isValidationError: false,
        isUnauthorizedError: false,
        message: '',
        type: 'error',
        code: null,
        details: [],
        timeout: 2500,
      };
      const ErrorMessages = {
        400: this.$i18n.t('general.status400Error'),
        401: this.$i18n.t('general.status401Error'),
        403: this.$i18n.t('general.status403Error'),
        404: this.$i18n.t('general.status404Error'),
        405: this.$i18n.t('general.status405Error'),
        422: this.$i18n.t('general.status422Error'),
        429: this.$i18n.t('general.status429Error'),
        500: this.$i18n.t('general.status500Error'),
        validationError:
          this.$i18n.t('general.validationError'),
        request:
          this.$i18n.t('general.requestError'),
        other:
          this.$i18n.t('general.otherError'),
      };
      if (Object.prototype.hasOwnProperty.call(customMessages, '400')) {
        ErrorMessages['400'] = customMessages['400'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '401')) {
        ErrorMessages['401'] = customMessages['401'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '403')) {
        ErrorMessages['403'] = customMessages['403'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '404')) {
        ErrorMessages['404'] = customMessages['404'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '405')) {
        ErrorMessages['405'] = customMessages['405'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '422')) {
        ErrorMessages['422'] = customMessages['422'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '429')) {
        ErrorMessages['429'] = customMessages['429'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, '500')) {
        ErrorMessages['500'] = customMessages['500'];
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, 'request')) {
        ErrorMessages.request = customMessages.request;
      }
      if (Object.prototype.hasOwnProperty.call(customMessages, 'other')) {
        ErrorMessages.other = customMessages.other;
      }
      if (error && error.response) {
        if (error.response.status === 400 && error.response.data.code === 'ValidationError') {
          errorResponse.isValidationError = true;
          errorResponse.code = error.response.data.code;
          errorResponse.details = error.response.data.details;
          errorResponse.message = ErrorMessages.validationError;
        } else if (error.response.status === 400) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['400'];
        } else if (error.response.status === 401) {
          errorResponse.isUnauthorizedError = true;
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['401'];
        } else if (error.response.status === 403) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['403'];
        } else if (error.response.status === 404) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['404'];
        } else if (error.response.status === 405) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['405'];
        } else if (error.response.status === 422) {
          errorResponse.isValidationError = true;
          errorResponse.code = error.response.data.code;
          errorResponse.details = error.response.data.details;
          errorResponse.message = ErrorMessages['422'];
        } else if (error.response.status >= 500) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['500'];
        } else if (error.response.status === 429) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['429'];
        }
      } else if (error && error.request) {
        errorResponse.message = ErrorMessages.request;
      } else if (error instanceof Error) {
        errorResponse.message = error.message;
      } else if (typeof error === 'string') {
        errorResponse.message = error;
      } else {
        errorResponse.message = ErrorMessages.other;
      }
      return errorResponse;
    },
  },
};
</script>