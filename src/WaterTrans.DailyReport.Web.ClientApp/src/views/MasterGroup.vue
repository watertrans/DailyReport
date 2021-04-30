<template>
  <div class="p-grid">
    <div class="p-col-12">
      <div class="card">
        <Toast/>
        <Toolbar class="p-mb-4">
          <template v-slot:left>
            <Button label="New" icon="pi pi-plus" class="p-button-success p-mr-2" @click="openNew" />
            <Button label="Delete" icon="pi pi-trash" class="p-button-danger p-mr-2" @click="confirmDeleteSelected" :disabled="!selectedGroups || !selectedGroups.length" />
            <span class="p-input-icon-left">
              <i class="pi pi-search" />
              <InputText v-model="query" placeholder="Search..." @keydown.enter="queryGroups" />
            </span>
          </template>

          <template v-slot:right>
            <FileUpload mode="basic" accept="image/*" :maxFileSize="1000000" label="Import" chooseLabel="Import" class="p-mr-2 p-d-inline-block" />
            <Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV($event)"  />
          </template>
        </Toolbar>

        <DataTable ref="dt" :value="groups" v-model:selection="selectedGroups" dataKey="groupId" :loading="loading" :filters="filters"
              paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,20,50,100]"
              currentPageReportTemplate="Showing {first} to {last} of {totalRecords} groups" responsiveLayout="scroll">
          <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
          <Column field="groupCode" header="コード" :sortable="true"></Column>
          <Column field="groupTree" header="階層" :sortable="true"></Column>
          <Column field="name" header="名前" :sortable="true"></Column>
          <Column field="status" header="ステータス"></Column>
          <Column field="sortNo" header="並び順" :sortable="true"></Column>
          <Column>
            <template #body="slotProps">
              <Button icon="pi pi-pencil" class="p-button-rounded p-button-success p-mr-2" @click="editGroup(slotProps.data)" />
              <Button icon="pi pi-trash" class="p-button-rounded p-button-warning" @click="confirmDeleteGroup(slotProps.data)" />
            </template>
          </Column>
        </DataTable>

        <Dialog v-model:visible="groupDialog" :style="{width: '450px'}" header="Group Details" :modal="true" class="p-fluid">
          <img :src="'assets/demo/images/group/' + group.image" :alt="group.image" class="group-image" v-if="group.image" />
          <div class="p-field">
            <label for="name">Name</label>
            <InputText id="name" v-model.trim="group.name" required="true" autofocus :class="{'p-invalid': submitted && !group.name}" />
            <small class="p-invalid" v-if="submitted && !group.name">Name is required.</small>
          </div>
          <div class="p-field">
            <label for="description">Description</label>
            <Textarea id="description" v-model="group.description" required="true" rows="3" cols="20" />
          </div>

          <div class="p-field">
            <label for="inventoryStatus" class="p-mb-3">Inventory Status</label>
            <Dropdown id="inventoryStatus" v-model="group.inventoryStatus" :options="statuses" optionLabel="label" placeholder="Select a Status">
              <template #value="slotProps">
                <div v-if="slotProps.value && slotProps.value.value">
                  <span :class="'group-badge status-' +slotProps.value.value">{{slotProps.value.label}}</span>
                </div>
                <div v-else-if="slotProps.value && !slotProps.value.value">
                  <span :class="'group-badge status-' +slotProps.value.toLowerCase()">{{slotProps.value}}</span>
                </div>
                <span v-else>
                  {{slotProps.placeholder}}
                </span>
              </template>
            </Dropdown>
          </div>

          <div class="p-field">
            <label class="p-mb-3">Category</label>
            <div class="p-formgrid p-grid">
              <div class="p-field-radiobutton p-col-6">
                <RadioButton id="category1" name="category" value="Accessories" v-model="group.category" />
                <label for="category1">Accessories</label>
              </div>
              <div class="p-field-radiobutton p-col-6">
                <RadioButton id="category2" name="category" value="Clothing" v-model="group.category" />
                <label for="category2">Clothing</label>
              </div>
              <div class="p-field-radiobutton p-col-6">
                <RadioButton id="category3" name="category" value="Electronics" v-model="group.category" />
                <label for="category3">Electronics</label>
              </div>
              <div class="p-field-radiobutton p-col-6">
                <RadioButton id="category4" name="category" value="Fitness" v-model="group.category" />
                <label for="category4">Fitness</label>
              </div>
            </div>
          </div>

          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="price">Price</label>
              <InputNumber id="price" v-model="group.price" mode="currency" currency="USD" locale="en-US" />
            </div>
            <div class="p-field p-col">
              <label for="quantity">Quantity</label>
              <InputNumber id="quantity" v-model="group.quantity" integeronly />
            </div>
          </div>
          <template #footer>
            <Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="hideDialog"/>
            <Button label="Save" icon="pi pi-check" class="p-button-text" @click="saveGroup" />
          </template>
        </Dialog>

        <Dialog v-model:visible="deleteGroupDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
          <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="group">Are you sure you want to delete <b>{{group.name}}</b>?</span>
          </div>
          <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteGroupDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteGroup" />
          </template>
        </Dialog>

        <Dialog v-model:visible="deleteGroupsDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
          <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="group">Are you sure you want to delete the selected groups?</span>
          </div>
          <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteGroupsDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteSelectedGroups" />
          </template>
        </Dialog>
      </div>
    </div>
  </div>

</template>

<script>
import ErrorHandling from '../mixins/ErrorHandling';
import GroupService from '../service/GroupService';

export default {
  data() {
    return {
      groups: null,
      groupDialog: false,
      deleteGroupDialog: false,
      deleteGroupsDialog: false,
      group: {},
      selectedGroups: null,
      filters: {},
      query: null,
      rows: 0,
      totalRecords: 0,
      loading: true,
      submitted: false,
      statuses: [
        {label: 'INSTOCK', value: 'instock'},
        {label: 'LOWSTOCK', value: 'lowstock'},
        {label: 'OUTOFSTOCK', value: 'outofstock'}
      ]
    };
  },
  mixins: [ErrorHandling],
  groupService: null,
  created() {
    this.groupService = new GroupService(this.$axios, this.$store.state.accessToken);
  },
  mounted() {
    this.queryGroups();
  },
  methods: {
    queryGroups(event) {
      if (!event || !event.isComposing) {
        this.groupService.queryGroups(this.query)
        .then(response => {
          this.groups = response.data.items;
          this.rows = response.data.items.length;
          this.totalRecords = response.data.total;
          this.loading = false;
        })
        .catch(error => {
          const errorResponse = this.handleError(error);
          if (errorResponse.isUnauthorizedError) {
            this.handleUnauthorizedError();
          } else if (errorResponse.isValidationError) {
            console.log(errorResponse.errors);
          } else {
            this.$toast.add({severity:'Error', summary: 'An error has occured.', detail:errorResponse.message, life: 5000});
          }
        });
      }
    },
    openNew() {
      this.group = {};
      this.submitted = false;
      this.groupDialog = true;
    },
    hideDialog() {
      this.groupDialog = false;
      this.submitted = false;
    },
    saveGroup() {
      this.submitted = true;
      if (this.group.name.trim()) {
      if (this.group.id) {
        this.group.inventoryStatus = this.group.inventoryStatus.value ? this.group.inventoryStatus.value: this.group.inventoryStatus;
        this.groups[this.findIndexById(this.group.id)] = this.group;
        this.$toast.add({severity:'success', summary: 'Successful', detail: 'Group Updated', life: 3000});
        }
        else {
          this.group.id = this.createId();
          this.group.code = this.createId();
          this.group.image = 'group-placeholder.svg';
          this.group.inventoryStatus = this.group.inventoryStatus ? this.group.inventoryStatus.value : 'INSTOCK';
          this.groups.push(this.group);
          this.$toast.add({severity:'success', summary: 'Successful', detail: 'Group Created', life: 3000});
        }
        this.groupDialog = false;
        this.group = {};
      }
    },
    editGroup(group) {
      this.group = {...group};
      this.groupDialog = true;
    },
    confirmDeleteGroup(group) {
      this.group = group;
      this.deleteGroupDialog = true;
    },
    deleteGroup() {
      this.groups = this.groups.filter(val => val.id !== this.group.id);
      this.deleteGroupDialog = false;
      this.group = {};
      this.$toast.add({severity:'success', summary: 'Successful', detail: 'Group Deleted', life: 3000});
    },
    findIndexById(id) {
      let index = -1;
      for (let i = 0; i < this.groups.length; i++) {
        if (this.groups[i].id === id) {
          index = i;
          break;
        }
      }
      return index;
    },
    createId() {
      let id = '';
      var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
      for ( var i = 0; i < 5; i++ ) {
        id += chars.charAt(Math.floor(Math.random() * chars.length));
      }
      return id;
    },
    exportCSV() {
      this.$refs.dt.exportCSV();
    },
    confirmDeleteSelected() {
      this.deleteGroupsDialog = true;
    },
    deleteSelectedGroups() {
      this.groups = this.groups.filter(val => !this.selectedGroups.includes(val));
      this.deleteGroupsDialog = false;
      this.selectedGroups = null;
      this.$toast.add({severity:'success', summary: 'Successful', detail: 'Groups Deleted', life: 3000});
    }
  }
};
</script>

<style scoped lang="scss">

  .p-dialog .group-image {
    width: 150px;
    margin: 0 auto 2rem auto;
    display: block;
  }

  .confirmation-content {
    display: flex;
    align-items: center;
    justify-content: center;
  }

  ::v-deep(.p-toolbar) {
    flex-wrap: wrap;
  }
</style>
