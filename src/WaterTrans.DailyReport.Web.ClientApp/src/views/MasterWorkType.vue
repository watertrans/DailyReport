<template>
  <div class="p-grid masterWorkType">
    <div class="p-col-12">
      <div class="card">
        <Toast/>
        <Toolbar class="p-mb-4">
          <template v-slot:left>
            <Button :label="$t('general.createButtonLabel')" icon="pi pi-plus" class="p-button-success p-mr-2" @click="openNew" />
            <Button :label="$t('general.updateSelectedButtonLabel')" icon="pi pi-tags" class="p-button-success p-mr-2" @click="updateSelected" :disabled="!selectedWorkTypes || !selectedWorkTypes.length" />
            <span class="p-input-icon-left">
              <i class="pi pi-search" />
              <InputText v-model="query" :placeholder="$t('general.search')" @keydown.enter="onSearchKeyDown" />
            </span>
          </template>

          <template v-slot:right>
            <Button icon="pi pi-upload" class="p-button-success p-mr-2" @click="showUploadDialog"  />
            <Button icon="pi pi-download" class="p-button-success" @click="exportCSV"  />
          </template>
        </Toolbar>

        <DataTable ref="dt" :value="workTypes" v-model:selection="selectedWorkTypes" dataKey="workTypeId" :lazy="true" :paginator="true" :rows="rows" :first="first"
              :totalRecords="totalRecords" :loading="loading" :resizableColumns="true" :removableSort="true" :sortOrder="sortOrder" :sortField="sortField"
              paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown" :rowsPerPageOptions="[5,10,20,50,100]"
              currentPageReportTemplate="Showing {first} to {last} of {totalRecords} workTypes" responsiveLayout="scroll" @page="onPage" @sort="onPage">
          <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
          <Column field="workTypeCode" :header="$t('schema.workType.workTypeCode')" headerStyle="width: 10rem" :sortable="true"></Column>
          <Column field="workTypeTree" :header="$t('schema.workType.workTypeTree')" headerStyle="width: 6rem" :sortable="true"></Column>
          <Column field="name" :header="$t('schema.workType.name')" :sortable="true"></Column>
          <Column field="tags" :header="$t('schema.workType.tags')" headerStyle="width: 15rem">
            <template #body="slotProps">
              <Chip v-for="tag in slotProps.data.tags" :key="tag" :label="tag" />
            </template>
          </Column>
          <Column field="status" :header="$t('schema.workType.status')" headerStyle="width: 8rem"></Column>
          <Column field="sortNo" :header="$t('schema.workType.sortNo')" headerStyle="width: 8rem" :sortable="true"></Column>
          <Column headerStyle="width: 10rem">
            <template #body="slotProps">
              <Button icon="pi pi-pencil" class="p-button-rounded p-button-outlined p-button-success p-mr-2" @click="editWorkType(slotProps.data)" />
              <Button icon="pi pi-trash" class="p-button-rounded p-button-outlined p-button-warning" @click="confirmDeleteWorkType(slotProps.data)" />
            </template>
          </Column>
        </DataTable>

        <Dialog v-model:visible="fileUploadDialog" :style="{width: '450px'}" :header="$t('masterWorkType.fileUploadDialogHeader')" :modal="true">
          <FileUpload mode="basic" :customUpload="true" :auto="true" accept="text/csv" :maxFileSize="1000000" :chooseLabel="$t('general.fileUploadDialogChooseButtonLabel')" @uploader="importCSV" class="p-d-inline-block" />
          <template #footer>
            <Button :label="$t('general.fileUploadDialogColumnsButtonLabel')" icon="pi pi-question-circle" class="p-button-text" />
            <Button :label="$t('general.fileUploadDialogDownloadSampleButtonLabel')" icon="pi pi-download" class="p-button-text" />
          </template>
        </Dialog>

        <Dialog v-model:visible="workTypeDialog" :style="{width: '450px'}" :header="workTypeDialogHeader" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="workTypeCode">{{$t('schema.workType.workTypeCode')}}</label>
              <InputText id="workTypeCode" v-model="workType.workTypeCode" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'workTypeCode') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'workTypeCode')">{{error.details.find(e => e.target == 'workTypeCode').message}}</small>
              <small class="help-text">{{$t('helpText.dataCode')}}</small>
            </div>
            <div class="p-field p-col">
              <label for="workTypeTree">{{$t('schema.workType.workTypeTree')}}</label>
              <InputText id="workTypeTree" v-model="workType.workTypeTree" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'workTypeTree') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'workTypeTree')">{{error.details.find(e => e.target == 'workTypeTree').message}}</small>
              <small class="help-text">{{$t('helpText.dataTree')}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="name">{{$t('schema.workType.name')}}</label>
            <InputText id="name" v-model="workType.name" required="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'name') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'name')">{{error.details.find(e => e.target == 'name').message}}</small>
            <small class="help-text">{{$t('helpText.text256')}}</small>
          </div>
          <div class="p-field">
            <label for="description">{{$t('schema.workType.description')}}</label>
            <Textarea id="description" v-model="workType.description" rows="3" cols="20" />
          </div>
          <div class="p-formgrid p-grid">
            <div class="p-field p-col">
              <label for="status">{{$t('schema.workType.status')}}</label>
              <Dropdown id="status" v-model="workType.status" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
            </div>
            <div class="p-field p-col">
              <label for="sortNo">{{$t('schema.workType.sortNo')}}</label>
              <InputNumber id="sortNo" v-model="workType.sortNo" :useGrouping="false" :min="0" :max="2147483647" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'sortNo') }" />
              <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'sortNo')">{{error.details.find(e => e.target == 'sortNo').message}}</small>
            </div>
          </div>
          <div class="p-field">
            <label for="tags">{{$t('schema.workType.tags')}}</label>
            <Chips id="tags" v-model="workType.tags" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button :label="$t('dialog.cancelButtonLabel')" icon="pi pi-times" class="p-button-text" @click="workTypeDialog = false"/>
            <Button :label="$t('dialog.saveButtonLabel')" icon="pi pi-check" class="p-button-text" @click="saveWorkType" />
          </template>
        </Dialog>

        <Dialog v-model:visible="deleteWorkTypeDialog" :style="{width: '450px'}" :header="$t('general.deleteComfirmTitle')" :modal="true">
          <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
            <span v-if="workType">{{$t('general.deleteComfirmMessage', { target : workType.name })}}</span>
          </div>
          <template #footer>
            <Button :label="$t('dialog.noButtonLabel')" icon="pi pi-times" class="p-button-text" @click="deleteWorkTypeDialog = false"/>
            <Button :label="$t('dialog.yesButtonLabel')" icon="pi pi-check" class="p-button-text" @click="deleteWorkType" />
          </template>
        </Dialog>

        <Dialog v-model:visible="updateSelectedDialog" :style="{width: '450px'}" :header="$t('masterWorkType.updateSelectedWorkTypeTitle')" :modal="true" class="p-fluid">
          <Message v-if="error && error.message" severity="error" :closable="false">{{error.message}}</Message>
          <div class="p-field">
            <label for="status2">{{$t('schema.workType.status')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedWorkType.statusChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Dropdown id="status2" v-model="updateSelectedWorkType.status" :disabled="!updateSelectedWorkType.statusChecked" :options="statuses" optionLabel="label" optionValue="value" :placeholder="$t('general.selectPlaceholder')" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'status') }" :style="{width: '100%'}" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'status')">{{error.details.find(e => e.target == 'status').message}}</small>
          </div>
          <div class="p-field">
            <label for="tags2">{{$t('schema.workType.tags')}}</label>
            <div class="p-formgrid p-grid">
              <div class="p-col-fixed">
                <ToggleButton v-model="updateSelectedWorkType.tagsChecked" onIcon="pi pi-check" offIcon="pi pi-times" />
              </div>
              <div class="p-col">
                <Chips id="tags2" v-model="updateSelectedWorkType.tags" :disabled="!updateSelectedWorkType.tagsChecked" :addOnBlur="true" :class="{'p-invalid': error && error.details && error.details.find(e => e.target == 'tags') }" />
              </div>
            </div>
            <small class="p-error" v-if="error && error.details && error.details.find(e => e.target == 'tags')">{{error.details.find(e => e.target == 'tags').message}}</small>
            <small class="help-text">{{$t('helpText.tags')}}</small>
          </div>
          <template #footer>
            <Button :label="$t('dialog.cancelButtonLabel')" icon="pi pi-times" class="p-button-text" @click="updateSelectedDialog = false"/>
            <Button :label="$t('dialog.updateButtonLabel')" icon="pi pi-check" class="p-button-text" @click="updateSelectedWorkTypes" />
          </template>
        </Dialog>
      </div>
    </div>
  </div>

</template>

<script>
import ErrorHandling from '../mixins/ErrorHandling';
import WorkTypeService from '../service/WorkTypeService';

export default {
  data() {
    return {
      workTypes: null,
      workTypeDialog: false,
      workTypeDialogHeader: null,
      fileUploadDialog: false,
      deleteWorkTypeDialog: false,
      updateSelectedDialog: false,
      workType: {},
      updateSelectedWorkType: {},
      selectedWorkTypes: null,
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
  workTypeService: null,
  created() {
    this.workTypeService = new WorkTypeService(this.$axios, this.$store.state.accessToken);
    if (!this.$route.query) {
      return;
    }
    if (this.$route.query.query) {
      this.query = this.$route.query.query;
    }
    if (this.$route.query.sort) {
      const firstChar = this.$route.query.sort.substring(0,1);
      if (firstChar === '-') {
        this.sortField = this.$route.query.sort.replace('-','');
        this.sortOrder = -1;
      } else {
        this.sortField = this.$route.query.sort;
        this.sortOrder = 1;
      }
    }
    if (this.$route.query.pageSize && !isNaN(this.$route.query.pageSize)) {
      this.rows = parseInt(this.$route.query.pageSize);
    }
    if (this.$route.query.page && !isNaN(this.$route.query.page)) {
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
      const queryParams = {};
      const pageSize = this.$refs.dt.d_rows;
      if (this.query) {
        queryParams.query = this.query;
      }
      if (sort) {
        queryParams.sort = sort;
      }
      if (page != 1) {
        queryParams.page = page;
      }
      if (pageSize != 20) {
        queryParams.pageSize = pageSize;
      }
      this.$router.push({ query: queryParams });
      this.queryWorkTypes(queryParams);
    },
    queryWorkTypes(queryParams) {
      this.loading = true;
      this.workTypeService.queryWorkTypes(queryParams)
        .then(response => {
          this.workTypes = response.data.items;
          this.rows = response.data.pageSize;
          this.totalRecords = response.data.total;
          this.loading = false;
        })
        .catch(error => this.handleErrorAndToastMessage(error));
    },
    openNew() {
      this.error = null;
      this.workType = { description: '', tags: [] };
      this.workTypeDialogHeader = this.$i18n.t('masterWorkType.createWorkTypeTitle');
      this.workTypeDialog = true;
    },
    editWorkType(workType) {
      this.error = null;
      this.workType = {...workType};
      this.workTypeDialogHeader = this.$i18n.t('masterWorkType.updateWorkTypeTitle');
      this.workTypeDialog = true;
    },
    saveWorkType() {
      this.error = { details: [] };

      if (!this.workType.workTypeCode) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'workTypeCode', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.workType.workTypeCode')}) });
      }
      if (!this.workType.workTypeTree) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'workTypeTree', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.workType.workTypeTree')}) });
      }
      if (!this.workType.name) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'name', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.workType.name')}) });
      }
      if (!this.workType.status) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'status', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.workType.status')}) });
      }
      if (!this.workType.sortNo && this.workType.sortNo !== 0) {
        this.error.message = this.$i18n.t('general.validationError');
        this.error.details.push({ target: 'sortNo', message: this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.workType.sortNo')}) });
      }

      if (this.error.message) {
        return;
      }

      if (this.workType.workTypeId) {
        this.workTypeService.updateWorkType(this.workType)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
            this.workTypeDialog = false;
            this.workType = response.data;
          })
          .catch(error => this.handleErrorAndDiplayMessage(error));
      } else {
        this.workTypeService.createWorkType(this.workType)
          .then(response => {
            this.reloadDataTable();
            this.$toast.add({severity:'success', summary: this.$i18n.t('toast.createSummary'), detail: this.$i18n.t('toast.createDetail'), life: 5000});
            this.workTypeDialog = false;
            this.workType = response.data;
          })
          .catch(error => this.handleErrorAndDiplayMessage(error));
      }
    },
    confirmDeleteWorkType(workType) {
      this.workType = workType;
      this.deleteWorkTypeDialog = true;
    },
    deleteWorkType() {
      this.workTypeService.deleteWorkType(this.workType.workTypeId)
        .then(() => {
          this.reloadDataTable();
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.deleteSummary'), detail: this.$i18n.t('toast.deleteDetail'), life: 5000});
          this.deleteWorkTypeDialog = false;
          this.workType = {};
        })
        .catch(error => {
          this.handleErrorAndToastMessage(error);
          this.reloadDataTable();
          this.deleteWorkTypeDialog = false;
          this.workType = {};
        });
    },
    exportCSV() {
      console.log('Not implemented!'); // TODO
    },
    showUploadDialog() {
      this.fileUploadDialog = true;
    },
    importCSV() {
      console.log('Not implemented!'); // TODO
    },
    updateSelected() {
      this.error = null;
      this.updateSelectedWorkType = { tags: []};
      this.updateSelectedDialog = true;
    },
    updateSelectedWorkTypes() {
      this.error = { details: [] };

      if (!this.updateSelectedWorkType.statusChecked && !this.updateSelectedWorkType.tagsChecked) {
        this.error.message = this.$i18n.t('general.updateSelectedRequired');
      }

      if (this.updateSelectedWorkType.statusChecked && !this.updateSelectedWorkType.status) {
        this.error.message = this.$i18n.t('general.validationRequired', { target: this.$i18n.t('schema.workType.status')});
      }

      if (this.error.message) {
        return;
      }

      var promises = [];
      this.selectedWorkTypes.forEach(workType => {
        var updateWorkType = { workTypeId: workType.workTypeId };
        if (this.updateSelectedWorkType.statusChecked) {
          updateWorkType.status = this.updateSelectedWorkType.status;
        }
        if (this.updateSelectedWorkType.tagsChecked) {
          updateWorkType.tags = this.updateSelectedWorkType.tags;
        }
        promises.push(
          this.workTypeService.updateWorkType(updateWorkType)
        );
      });

      Promise.all(promises)
        .then(() => {
          this.$toast.add({severity:'success', summary: this.$i18n.t('toast.updateSummary'), detail: this.$i18n.t('toast.updateDetail'), life: 5000});
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedWorkType = {};
          this.selectedWorkTypes = null;
        })
        .catch(error => {
          this.handleErrorAndToastMessage(error);
          this.reloadDataTable();
          this.updateSelectedDialog = false;
          this.updateSelectedWorkType = {};
        });
    }
  }
};
</script>
