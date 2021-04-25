<template>
  <div :class="containerClass" @click="onWrapperClick">
    <AppTopBar @menu-toggle="onMenuToggle" />
    <transition name="layout-sidebar">
      <div :class="sidebarClass" @click="onSidebarClick" v-show="isSidebarVisible()">
        <div class="layout-logo">
          <router-link to="/">
            <img alt="Logo" :src="logo" />
          </router-link>
        </div>
        <AppMenu :model="menu" @menuitem-click="onMenuItemClick" />
      </div>
    </transition>
    <div class="layout-main">
      <router-view />
    </div>
    <AppFooter />
  </div>
</template>

<script>
import AuthService from './service/AuthService';
import AppTopBar from './AppTopbar.vue';
import AppMenu from './AppMenu.vue';
import AppFooter from './AppFooter.vue';

export default {
  data() {
    return {
      layoutMode: 'static',
      layoutColorMode: 'dark',
      staticMenuInactive: false,
      overlayMenuActive: false,
      mobileMenuActive: false,
      menu : [
        {label: 'ダッシュボード', icon: 'pi pi-fw pi-home', to: '/'},
        {label: '未提出', icon: 'pi pi-fw pi-exclamation-circle', to: '/search?unreported=true'},
        {label: '全未読', icon: 'pi pi-fw pi-check-square', to: '/search?unread=true'},
        {label: 'スレッド', icon: 'pi pi-fw pi-comments', to: '/thread'},
        {label: 'メンション＆リアクション', icon: 'pi pi-fw pi-comment', to: '/reaction'},
        {label: 'ブックマーク', icon: 'pi pi-fw pi-bookmark', to: '/bookmark'},
        {label: '検索', icon: 'pi pi-fw pi-search', to: '/search'},
        {
          label: 'マスタメンテ', icon:'pi pi-fw pi-book',
          items: [
            {label: '部署', icon:'pi pi-fw pi-sitemap', to:'/master/group'},
            {label: 'プロジェクト', icon:'pi pi-fw pi-briefcase', to:'/master/project'},
            {label: '従業員', icon:'pi pi-fw pi-users', to:'/master/person'},
            {label: '業務', icon:'pi pi-fw pi-desktop', to:'/master/workType'}
          ]
        },
        {
          label: '分析', icon:'pi pi-fw pi-globe',
          items: [
            {label: '部署', icon:'pi pi-fw pi-sitemap', to:'/analysis/group'},
            {label: 'プロジェクト', icon:'pi pi-fw pi-briefcase', to:'/analysis/project'},
            {label: '従業員', icon:'pi pi-fw pi-users', to:'/analysis/person'},
            {label: '業務', icon:'pi pi-fw pi-desktop', to:'/analysis/workType'},
            {label: 'いいね', icon:'pi pi-fw pi-thumbs-up', to:'/analysis/likeIt'},
            {label: 'リアクション', icon:'pi pi-fw pi-comment', to:'/analysis/reaction'}
          ]
        }
      ]
    };
  },
  authService: null,
  created() {
    this.$store.commit('initialiseStore');
    this.authService = new AuthService();
  },
  watch: {
    $route() {
      if (this.$route.query.code && this.$route.query.code !== this.$store.state.authorizationCode) {
        this.authService.getAccessToken(this.$route.query.code)
          .then(response => {
            this.$store.commit('setAccessToken', response.data.access_token);
            this.$store.commit('setAuthorizationCode', this.$route.query.code);
          })
          .catch(error => console.log('status:' + error.response.status)); // TODO エラー時にトーストを表示する？
      }
      this.menuActive = false;
      this.$toast.removeAllGroups();
    }
  },
  methods: {
      onWrapperClick() {
          if (!this.menuClick) {
              this.overlayMenuActive = false;
              this.mobileMenuActive = false;
          }

          this.menuClick = false;
      },
      onMenuToggle() {
          this.menuClick = true;

          if (this.isDesktop()) {
              if (this.layoutMode === 'overlay') {
        if(this.mobileMenuActive === true) {
          this.overlayMenuActive = true;
        }

                  this.overlayMenuActive = !this.overlayMenuActive;
        this.mobileMenuActive = false;
              }
              else if (this.layoutMode === 'static') {
                  this.staticMenuInactive = !this.staticMenuInactive;
              }
          }
          else {
              this.mobileMenuActive = !this.mobileMenuActive;
          }

          event.preventDefault();
      },
      onSidebarClick() {
          this.menuClick = true;
      },
      onMenuItemClick(event) {
          if (event.item && !event.item.items) {
              this.overlayMenuActive = false;
              this.mobileMenuActive = false;
          }
      },
  onLayoutChange(layoutMode) {
    this.layoutMode = layoutMode;
  },
  onLayoutColorChange(layoutColorMode) {
    this.layoutColorMode = layoutColorMode;
  },
      addClass(element, className) {
          if (element.classList)
              element.classList.add(className);
          else
              element.className += ' ' + className;
      },
      removeClass(element, className) {
          if (element.classList)
              element.classList.remove(className);
          else
              element.className = element.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
      },
      isDesktop() {
          return window.innerWidth > 1024;
      },
      isSidebarVisible() {
          if (this.isDesktop()) {
              if (this.layoutMode === 'static')
                  return !this.staticMenuInactive;
              else if (this.layoutMode === 'overlay')
                  return this.overlayMenuActive;
              else
                  return true;
          }
          else {
              return true;
          }
      },
  },
  computed: {
      containerClass() {
          return ['layout-wrapper', {
              'layout-overlay': false,
              'layout-static': true,
              'layout-static-sidebar-inactive': this.staticMenuInactive && this.layoutMode === 'static',
              'layout-overlay-sidebar-active': this.overlayMenuActive && this.layoutMode === 'overlay',
              'layout-mobile-sidebar-active': this.mobileMenuActive,
              'p-input-filled': false,
              'p-ripple-disabled': false
          }];
      },
      sidebarClass() {
          return ['layout-sidebar', {
              'layout-sidebar-dark': false,
              'layout-sidebar-light': true
          }];
      },
      logo() {
          return 'assets/layout/images/logo.svg';
      }
  },
  beforeUpdate() {
      if (this.mobileMenuActive)
          this.addClass(document.body, 'body-overflow-hidden');
      else
          this.removeClass(document.body, 'body-overflow-hidden');
  },
  components: {
      'AppTopBar': AppTopBar,
      'AppMenu': AppMenu,
      'AppFooter': AppFooter
  }
};
</script>

<style lang="scss">
@import './App.scss';
</style>
