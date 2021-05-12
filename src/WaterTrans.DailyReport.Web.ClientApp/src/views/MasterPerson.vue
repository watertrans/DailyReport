<template>
  <div class="p-grid masterPerson">
    <div class="p-col-12">
      <div class="card">
        <Toast/>
        <Toolbar class="p-mb-4">
          <template v-slot:left>
            <Button :label="$t('general.createButtonLabel')" icon="pi pi-plus" class="p-button-success p-mr-2" @click="openNew" />
            <Button :label="$t('general.updateSelectedButtonLabel')" icon="pi pi-tags" class="p-button-success p-mr-2" @click="updateSelected" :disabled="!selectedPersons || !selectedPersons.length" />
            <span class="p-input-icon-left">
              <i class="pi pi-search" />
              <InputText v-model="query" placeholder="Search..." @keydown.enter="onSearchKeyDown" />
            </span>
          </template>

          <template v-slot:right>
            <FileUpload mode="basic" :customUpload="true" :auto="true" accept="text/csv" :maxFileSize="1000000" label="Import" chooseLabel="Import" @uploader="importCSV" class="p-mr-2 p-d-inline-block" />
            <Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV"  />
          </template>
        </Toolbar>

        <DataTable ref="dt" :value="persons" v-model:selection="selectedPersons" dataKey="personId" :lazy="true" :paginator="true" :rows="rows" :first="first"
              :totalRecords="totalRecords" :loading="loading" :resizableColumns="true" :removableSort="true" :sortOrder="sortOrder" :sortField="sortField"
              paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,20,50,100]"
              currentPageReportTemplate="Showing {first} to {last} of {totalRecords} persons" responsiveLayout="scroll" @page="onPage" @sort="onPage">
          <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
          <Column field="personCode" :header="$t('schema.person.personCode')" headerStyle="width: 10rem" :sortable="true"></Column>
          <Column field="loginId" :header="$t('schema.person.loginId')" headerStyle="width: 20rem"></Column>
          <Column field="name" :header="$t('schema.person.name')" :sortable="true"></Column>
          <Column field="title" :header="$t('schema.person.title')" headerStyle="width: 15rem"></Column>
          <Column field="tags" :header="$t('schema.person.tags')" headerStyle="width: 15rem">
            <template #body="slotProps">
              <Chip v-for="tag in slotProps.data.tags" :key="tag" :label="tag" />
            </template>
          </Column>
          <Column field="status" :header="$t('schema.person.status')" headerStyle="width: 8rem"></Column>
          <Column field="sortNo" :header="$t('schema.person.sortNo')" headerStyle="width: 8rem" :sortable="true"></Column>
          <Column headerStyle="width: 10rem">
            <template #body="slotProps">
              <Button icon="pi pi-pencil" class="p-button-rounded p-button-outlined p-button-success p-mr-2" @click="editPerson(slotProps.data)" />
              <Button icon="pi pi-trash" class="p-button-rounded p-button-outlined p-button-warning" @click="confirmDeletePerson(slotProps.data)" />
            </template>
          </Column>
        </DataTable>

        <Dialog v-model:visible="personDialog" :style="{width: '450px'}" :header="personDialogHeader" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="personCode">{{$t('schema.person.personCode')}}</label>
              <InputText id="personCode" v-model="person.personCode" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'personCode') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'personCode')">{{error.details.find(e => e.target == 'personCode').message}}</small>
              <small class="help-text">{{$t('helpText.dataCode')}}</small>
            </div>
            <div class="p-field p-col">
              <label for="title">{{$t('schema.person.title')}}</label>
              <InputText id="title" v-model="person.title" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'title') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'title')">{{error.details.find(e => e.target == 'title').message}}</small>
              <small class="help-text">{{$t('helpText.text100')}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="loginId">{{$t('schema.person.loginId')}}</label>
            <InputText id="loginId" v-model="person.loginId" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'loginId') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'loginId')">{{error.details.find(e => e.target == 'loginId').message}}</small>
            <small class="help-text">{{$t('helpText.text256')}}</small>
          </div>
          <div class="p-field">
            <label for="name">{{$t('schema.person.name')}}</label>
            <InputText id="name" v-model="person.name" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'name') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'name')">{{error.details.find(e => e.target == 'name').message}}</small>
            <small class="help-text">{{$t('helpText.text256')}}</small>
          </div>
          <div class="p-field">
            <label for="description">{{$t('schema.person.description')}}</label>
            <Textarea id="description" v-model="person.description" rows="3" cols="20" />
          </div>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="status">{{$t('schema.person.status')}}</label>
              <Dropdown id="status" v-model="person.status" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
            </div>
            <div class="p-field p-col">
              <label for="sortNo">{{$t('schema.person.sortNo')}}</label>
              <InputNumber id="sortNo" v-model="person.sortNo" :useGrouping="false" :min="0" :max="2147483647" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'sortNo') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'sortNo')">{{error.details.find(e => e.target == 'sortNo').message}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="tags">{{$t('schema.person.tags')}}</label>
            <Chips id="tags" v-model="person.tags" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="personDialog = false"/>
            <Button label="Save" icon="pi pi-check" class="p-button-text" @click="savePerson" />
          </template>
        </Dialog>

        <Dialog v-model:visible="deletePersonDialog" :style="{width: '450px'}" :header="$t('general.deleteComfirmTitle')" :modal="true">
          <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="person">{{$t('general.deleteComfirmMessage', { target : person.name })}}</span>
          </div>
          <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="deletePersonDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deletePerson" />
          </template>
        </Dialog>

        <Dialog v-model:visible="updateSelectedDialog" :style="{width: '450px'}" :header="$t('masterPerson.updateSelectedPersonTitle')" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-field">
            <label for="status2">{{$t('schema.person.status')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedPerson.statusChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Dropdown id="status2" v-model="updateSelectedPerson.status" :disabled="!updateSelectedPerson.statusChecked" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" :style="{width: '100%'}" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
          </div>
          <div class="p-field">
            <label for="tags2">{{$t('schema.person.tags')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedPerson.tagsChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Chips id="tags2" v-model="updateSelectedPerson.tags" :disabled="!updateSelectedPerson.tagsChecked" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="updateSelectedDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="updateSelectedPersons" />
          </template>
        </Dialog>
      </div>
    </div>
  </div>

</template>

<script>
import ErrorHandling from '../mixins/ErrorHandling';
import PersonService from '../service/PersonService';

export default {
  data() {
    return {
      persons: null,
      personDialog: false,
      personDialogHeader: null,
      deletePersonDialog: false,
      updateSelectedDialog: false,
      person: {},
      updateSelectedPerson: {},
      selectedPersons: null,
      query: null,
      sortOrder: 1,
      sortField: null,
      first: 0,
      rows: 20,
      totalRecords: 0,
      loading: true,
      error: null,
      statuses: [
        {label: 'NORMAL', value: 'NORMAL'},
        {label: 'SUSPENDED', value: 'SUSPENDED'}
      ]
    };
  },
  mixins: [ErrorHandling],
  personService: null,
  created() {
    this.personService = new PersonService(this.$axios, this.$store.state.accessToken);
    if (this.$route.query && this.$route.query.q)
    {
      this.query = this.$route.query.q;
    }
    if (this.$route.query && this.$route.query.sort)
    {
      const firstChar = this.$route.query.sort.substring(0,1);
      if (firstChar === '-') {
        this.sortField = this.$route.query.sort.replace('-','');
        this.sortOrder = -1;
      } else {
        this.sortField = this.$route.query.sort;
        this.sortOrder = 1;
      }
    }
    if (this.$route.query && this.$route.query.pageSize && !isNaN(this.$route.query.pageSize))
    {
      this.rows = parseInt(this.$route.query.pageSize);
    }
    if (this.$route.query && this.$route.query.page && !isNaN(this.$route.query.page))
    {
      this.first = (this.rows * parseInt(this.$route.query.page)) - 1;
    }
  },
  mounted() {
    this.reloadDataTable();
  },
  methods: {
    onSearchKeyDown(event) {
      if (!event || !event.isComposing) {
        this.reloadDataTable();
      }
    },
    onPage(event) {
      var page = 1;
      if (!isNaN(event.page)) {
        page = event.page + 1;
      }
      this.loadDataTable(this.convertToSortReuqest(event), page);
    },
    reloadDataTable() {
      this.loadDataTable(this.convertToSortReuqest({ 
        sortField: this.$refs.dt.d_sortField,
        sortOrder: this.$refs.dt.d_sortOrder
      }), Math.floor(this.$refs.dt.first / this.$refs.dt.d_rows) + 1);
    },
    loadDataTable(sort = null, page = 1) {
      const routerQuery = {};
      const pageSize = this.$refs.dt.d_rows;
      if (this.query) {
        routerQuery.q = this.query;
      }
      if (sort) {
        routerQuery.sort = sort;
      }
      if (page != 1) {
        routerQuery.page = page;
      }
      if (pageSize != 20) {
        routerQuery.pageSize = pageSize;
      }
      this.$router.push({ query: routerQuery });
      this.queryPersons(routerQuery.q, routerQuery.sort, routerQuery.page, routerQuery.pageSize);
    },
    queryPersons(query, sort, page, pageSize) {
      this.loading = true;
      this.personService.queryPersons(query, sort, page, pageSize)
        .then(response => {
          this.persons = response.data.items;
          this.rows = response.data.pageSize;
          this.totalRecords = response.data.total;
          this.loading = false;
        })
        .catch(error => {
          const errorResponse = this.handleError(error);
          if (errorResponse.isUnauthorizedError) {
            this.handleUnauthorizedError();
          } else {
            this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
          }
        });
    },
    openNew() {
      this.error = null;
      this.person = { title: '', description: '', tags: [] };
      this.personDialogHeader = this.$i18n.t('masterPerson.createPersonTitle');
      this.personDialog = true;
    },
    editPerson(person) {
      this.error = null;
      this.person = {...person};
      this.personDialogHeader = this.$i18n.t('masterPerson.updatePersonTitle');
      this.personDialog = true;
    },
    savePerson() {
      this.error = { details: [] };

      if (!this.person.personCode) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'personCode', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.person.personCode')}) });
      }
      if (!this.person.loginId) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'loginId', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.person.loginId')}) });
      }
      if (!this.person.name) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'name', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.person.name')}) });
      }
      if (!this.person.status) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'status', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.person.status')}) });
      }
      if (!this.person.sortNo && this.person.sortNo !== 0) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'sortNo', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.person.sortNo')}) });
      }

      if (this.error.message) {
        return;
      }

      if (this.person.personId) {
        this.personService.updatePerson(this.person)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
            this.personDialog = false;
            this.person = response.data;
          })
          .catch(error => {
            const errorResponse = this.handleError(error);
            if (errorResponse.isUnauthorizedError) {
              this.handleUnauthorizedError();
            } else if (errorResponse.isValidationError) {
              this.error.message = errorResponse.message;
              this.error.details = errorResponse.details;
            } else {
              this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
            }
          });
      } else {
        this.personService.createPerson(this.person)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.createSummary'), detail: this.$i18n.t('toast.createDetail'), life: 5000});
            this.personDialog = false;
            this.person = response.data;
          })
          .catch(error => {
            const errorResponse = this.handleError(error);
            if (errorResponse.isUnauthorizedError) {
              this.handleUnauthorizedError();
            } else if (errorResponse.isValidationError) {
              this.error.message = errorResponse.message;
              this.error.details = errorResponse.details;
            } else {
              this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
            }
          });
      }
    },
    confirmDeletePerson(person) {
      this.person = person;
      this.deletePersonDialog = true;
    },
    deletePerson() {
      this.personService.deletePerson(this.person.personId)
        .then(() => {
          this.reloadDataTable();
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.deleteSummary'), detail: this.$i18n.t('toast.deleteDetail'), life: 5000});
          this.deletePersonDialog = false;
          this.person = {};
        })
        .catch(error => {
          const errorResponse = this.handleError(error);
          if (errorResponse.isUnauthorizedError) {
            this.handleUnauthorizedError();
          } else if (errorResponse.isValidationError) {
            this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
            errorResponse.details.forEach(element => {
              this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorDetail'), detail:element.message, life: 5000});
            });
          } else {
            this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
          }
          this.reloadDataTable();
          this.deletePersonDialog = false;
          this.person = {};
        });
    },
    exportCSV() {
      console.log('Not implemented!'); // TODO
    },
    importCSV() {
      console.log('Not implemented!'); // TODO
    },
    updateSelected() {
      this.error = null;
      this.updateSelectedPerson = { tags: []};
      this.updateSelectedDialog = true;
    },
    updateSelectedPersons() {
      this.error = { details: [] };

      if (!this.updateSelectedPerson.statusChecked && !this.updateSelectedPerson.tagsChecked) {
        this.error.message = this.$i18n.t('general.updateSelectedRequired');
      }

      if (this.updateSelectedPerson.statusChecked && !this.updateSelectedPerson.status) {
        this.error.message = this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.person.status')});
      }

      if (this.error.message) {
        return;
      }

      var promises = [];
      this.selectedPersons.forEach(person => {
        var updatePerson = { personId: person.personId };
        if (this.updateSelectedPerson.statusChecked) {
          updatePerson.status = this.updateSelectedPerson.status;
        }
        if (this.updateSelectedPerson.tagsChecked) {
          updatePerson.tags = this.updateSelectedPerson.tags;
        }
        promises.push(
          this.personService.updatePerson(updatePerson)
        );
      });

      Promise.all(promises)
        .then(() => {
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedPerson = {};
          this.selectedPersons = null;
        })
        .catch(error => {
          const errorResponse = this.handleError(error);
          if (errorResponse.isUnauthorizedError) {
            this.handleUnauthorizedError();
          } else {
            this.$toast.add({severity:'error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
          }
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedPerson = {};
        });
    }
  }
};
</script>

<style scoped lang="scss">

  .confirmation-content {
    display: flex;
    align-items: center;
    justify-content: center;
  }

</style>
