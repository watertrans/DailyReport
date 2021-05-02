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
        message: 'Network Error.',
        type: 'error',
        code: null,
        details: [],
        timeout: 2500,
      };
      const ErrorMessages = {
        400: 'There was Some Problem, while processing your Request', // not being used currently
        401: 'Unauthorized, You are not Allowed',
        403: 'Sorry, You are not allowed for This Action',
        404: 'API Route is Missing or Undefined',
        405: 'API Route Method Not Allowed',
        429: 'Too Many Requests',
        500: 'Server Error, please try again later',
        request:
          'There is Some Problem With Our Servers, Please Try again Later',
        other:
          'There was some Problem with your Request, Please Try again Later',
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
        if (error.response.status === 400) {
          errorResponse.isValidationError = true;
          errorResponse.code = error.response.data.code;
          errorResponse.details = error.response.data.details;
          errorResponse.message = error.response.data.message;
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
        } else if (error.response.status === 422) {
          errorResponse.isValidationError = true;
          errorResponse.code = error.response.data.code;
          errorResponse.details = error.response.data.details;
          errorResponse.message = error.response.data.message;
        } else if (error.response.status === 405) {
          errorResponse.code = error.response.data.code;
          errorResponse.message = ErrorMessages['405'];
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