<template>
  <AutoComplete v-model="selectedGroup" :suggestions="filteredGroups" @complete="searchGroup($event)" @item-select="itemSelect($event)" field="groupCode" :delay="500" :placeholder="$t('general.search')">
    <template #item="slotProps">
      <div>{{slotProps.item.label}}</div>
    </template>
  </AutoComplete>
</template>

<script>
import ErrorHandling from '../mixins/ErrorHandling';
import GroupService from '../service/GroupService';
export default {
  data() {
    return {
      filteredGroups: null,
    };
  },
  props: {
    modelValue: null
  },
  mixins: [ErrorHandling],
  groupService: null,
  created() {
    this.groupService = new GroupService(this.$axios, this.$store.state.accessToken);
  },
  mounted() {
  },
  methods: {
    searchGroup(event) {
      if (event.query.trim().length == 0) {
        this.filteredGroups = null;
        return;
      }
      this.groupService.queryGroups(event.query)
        .then(response => {
          response.data.items.forEach(group => {
            group.label = group.groupCode + ':' + group.name;
          });
          this.filteredGroups = response.data.items;
        })
        .catch(error => {
          const errorResponse = this.handleErrorResponse(error);
          if (errorResponse.isUnauthorizedError) {
            this.handleUnauthorizedError();
          } else {
            this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
          }
        });
    },
    itemSelect(event) {
      this.$emit('update:modelValue', event.value.groupCode);
    }
  },
  computed: {
    selectedGroup: {
      get () {
        return this.modelValue;
      },
      set (value) {
        this.$emit('update:modelValue', value);
      }
    }
  }
};
</script>