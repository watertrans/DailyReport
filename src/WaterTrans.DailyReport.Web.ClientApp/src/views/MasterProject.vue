<template>
  <div class="p-grid masterProject">
    <div class="p-col-12">
      <div class="card">
        <Toast/>
        <Toolbar class="p-mb-4">
          <template v-slot:left>
            <Button :label="$t('general.createButtonLabel')" icon="pi pi-plus" class="p-button-success p-mr-2" @click="openNew" />
            <Button :label="$t('general.updateSelectedButtonLabel')" icon="pi pi-tags" class="p-button-success p-mr-2" @click="updateSelected" :disabled="!selectedProjects || !selectedProjects.length" />
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

        <DataTable ref="dt" :value="projects" v-model:selection="selectedProjects" dataKey="projectId" :lazy="true" :paginator="true" :rows="rows" :first="first"
              :totalRecords="totalRecords" :loading="loading" :resizableColumns="true" :removableSort="true" :sortOrder="sortOrder" :sortField="sortField"
              paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,20,50,100]"
              currentPageReportTemplate="Showing {first} to {last} of {totalRecords} projects" responsiveLayout="scroll" @page="onPage" @sort="onPage">
          <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
          <Column field="projectCode" :header="$t('schema.project.projectCode')" headerStyle="width: 10rem" :sortable="true"></Column>
          <Column field="name" :header="$t('schema.project.name')" :sortable="true"></Column>
          <Column field="tags" :header="$t('schema.project.tags')" headerStyle="width: 15rem">
            <template #body="slotProps">
              <Chip v-for="tag in slotProps.data.tags" :key="tag" :label="tag" />
            </template>
          </Column>
          <Column field="status" :header="$t('schema.project.status')" headerStyle="width: 8rem"></Column>
          <Column field="sortNo" :header="$t('schema.project.sortNo')" headerStyle="width: 8rem" :sortable="true"></Column>
          <Column field="persons.length" :header="$t('schema.project.persons')" headerStyle="width: 8rem"></Column>
          <Column headerStyle="width: 10rem">
            <template #body="slotProps">
              <Button icon="pi pi-pencil" class="p-button-rounded p-button-outlined p-button-success p-mr-2" @click="editProject(slotProps.data)" />
              <Button icon="pi pi-trash" class="p-button-rounded p-button-outlined p-button-warning" @click="confirmDeleteProject(slotProps.data)" :disabled="slotProps.data.persons.length > 0" />
            </template>
          </Column>
        </DataTable>

        <Dialog v-model:visible="projectDialog" :style="{width: '450px'}" :header="projectDialogHeader" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="projectCode">{{$t('schema.project.projectCode')}}</label>
              <InputText id="projectCode" v-model="project.projectCode" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'projectCode') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'projectCode')">{{error.details.find(e => e.target == 'projectCode').message}}</small>
              <small class="help-text">{{$t('helpText.dataCode')}}</small>
            </div>
            <div class="p-field p-col">
            </div>
          </div>
          <div class="p-field">
            <label for="name">{{$t('schema.project.name')}}</label>
            <InputText id="name" v-model="project.name" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'name') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'name')">{{error.details.find(e => e.target == 'name').message}}</small>
            <small class="help-text">{{$t('helpText.text256')}}</small>
          </div>
          <div class="p-field">
            <label for="description">{{$t('schema.project.description')}}</label>
            <Textarea id="description" v-model="project.description" rows="3" cols="20" />
          </div>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="status">{{$t('schema.project.status')}}</label>
              <Dropdown id="status" v-model="project.status" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
            </div>
            <div class="p-field p-col">
              <label for="sortNo">{{$t('schema.project.sortNo')}}</label>
              <InputNumber id="sortNo" v-model="project.sortNo" :useGrouping="false" :min="0" :max="2147483647" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'sortNo') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'sortNo')">{{error.details.find(e => e.target == 'sortNo').message}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="tags">{{$t('schema.project.tags')}}</label>
            <Chips id="tags" v-model="project.tags" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="projectDialog = false"/>
            <Button label="Save" icon="pi pi-check" class="p-button-text" @click="saveProject" />
          </template>
        </Dialog>

        <Dialog v-model:visible="deleteProjectDialog" :style="{width: '450px'}" :header="$t('general.deleteComfirmTitle')" :modal="true">
          <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="project">{{$t('general.deleteComfirmMessage', { target : project.name })}}</span>
          </div>
          <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="deleteProjectDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteProject" />
          </template>
        </Dialog>

        <Dialog v-model:visible="updateSelectedDialog" :style="{width: '450px'}" :header="$t('masterProject.updateSelectedProjectTitle')" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-field">
            <label for="status2">{{$t('schema.project.status')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedProject.statusChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Dropdown id="status2" v-model="updateSelectedProject.status" :disabled="!updateSelectedProject.statusChecked" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" :style="{width: '100%'}" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
          </div>
          <div class="p-field">
            <label for="tags2">{{$t('schema.project.tags')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedProject.tagsChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Chips id="tags2" v-model="updateSelectedProject.tags" :disabled="!updateSelectedProject.tagsChecked" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button label="No" icon="pi pi-times" class="p-button-text" @click="updateSelectedDialog = false"/>
            <Button label="Yes" icon="pi pi-check" class="p-button-text" @click="updateSelectedProjects" />
          </template>
        </Dialog>
      </div>
    </div>
  </div>

</template>

<script>
import ErrorHandling from '../mixins/ErrorHandling';
import ProjectService from '../service/ProjectService';

export default {
  data() {
    return {
      projects: null,
      projectDialog: false,
      projectDialogHeader: null,
      deleteProjectDialog: false,
      updateSelectedDialog: false,
      project: {},
      updateSelectedProject: {},
      selectedProjects: null,
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
        {label: 'SUSPENDED', value: 'SUSPENDED'},
        {label: 'COMPLETED', value: 'COMPLETED'}
      ]
    };
  },
  mixins: [ErrorHandling],
  projectService: null,
  created() {
    this.projectService = new ProjectService(this.$axios, this.$store.state.accessToken);
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
      this.queryProjects(routerQuery.q, routerQuery.sort, routerQuery.page, routerQuery.pageSize);
    },
    queryProjects(query, sort, page, pageSize) {
      this.loading = true;
      this.projectService.queryProjects(query, sort, page, pageSize)
        .then(response => {
          this.projects = response.data.items;
          this.rows = response.data.pageSize;
          this.totalRecords = response.data.total;
          this.loading = false;
        })
        .catch(error => {
          const errorResponse = this.handleError(error);
          if (errorResponse.isUnauthorizedError) {
            this.handleUnauthorizedError();
          } else {
            this.$toast.add({severity:'Error', summary: this.$i18n.t('toast.errorSummary'), detail:errorResponse.message, life: 5000});
          }
        });
    },
    openNew() {
      this.error = null;
      this.project = { description: '', tags: [] };
      this.projectDialogHeader = this.$i18n.t('masterProject.createProjectTitle');
      this.projectDialog = true;
    },
    editProject(project) {
      this.error = null;
      this.project = {...project};
      this.projectDialogHeader = this.$i18n.t('masterProject.updateProjectTitle');
      this.projectDialog = true;
    },
    saveProject() {
      this.error = { details: [] };

      if (!this.project.projectCode) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'projectCode', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.project.projectCode')}) });
      }
      if (!this.project.name) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'name', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.project.name')}) });
      }
      if (!this.project.status) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'status', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.project.status')}) });
      }
      if (!this.project.sortNo && this.project.sortNo !== 0) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'sortNo', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.project.sortNo')}) });
      }

      if (this.error.message) {
        return;
      }

      if (this.project.projectId) {
        this.projectService.updateProject(this.project)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
            this.projectDialog = false;
            this.project = response.data;
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
        this.projectService.createProject(this.project)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.createSummary'), detail: this.$i18n.t('toast.createDetail'), life: 5000});
            this.projectDialog = false;
            this.project = response.data;
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
    confirmDeleteProject(project) {
      this.project = project;
      this.deleteProjectDialog = true;
    },
    deleteProject() {
      this.projectService.deleteProject(this.project.projectId)
        .then(() => {
          this.reloadDataTable();
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.deleteSummary'), detail: this.$i18n.t('toast.deleteDetail'), life: 5000});
          this.deleteProjectDialog = false;
          this.project = {};
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
          this.deleteProjectDialog = false;
          this.project = {};
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
      this.updateSelectedProject = { tags: []};
      this.updateSelectedDialog = true;
    },
    updateSelectedProjects() {
      this.error = { details: [] };

      if (!this.updateSelectedProject.statusChecked && !this.updateSelectedProject.tagsChecked) {
        this.error.message = this.$i18n.t('general.updateSelectedRequired');
      }

      if (this.updateSelectedProject.statusChecked && !this.updateSelectedProject.status) {
        this.error.message = this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.project.status')});
      }

      if (this.error.message) {
        return;
      }

      var promises = [];
      this.selectedProjects.forEach(project => {
        var updateProject = { projectId: project.projectId };
        if (this.updateSelectedProject.statusChecked) {
          updateProject.status = this.updateSelectedProject.status;
        }
        if (this.updateSelectedProject.tagsChecked) {
          updateProject.tags = this.updateSelectedProject.tags;
        }
        promises.push(
          this.projectService.updateProject(updateProject)
        );
      });

      Promise.all(promises)
        .then(() => {
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedProject = {};
          this.selectedProjects = null;
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
          this.updateSelectedProject = {};
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
