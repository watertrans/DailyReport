<script>
export default {
  methods: {
    buildSortReuqest(event) {
      var result = event.sortField;
      if (event.sortOrder < 0) {
        result = '-' + result;
      }
      return result;
    },
  },
};
</script>