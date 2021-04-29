import { createI18n } from 'vue-i18n';

const messages = {
  en: {
    menu: {
      dashborad: 'Dashborad',
      unreported: 'Not yet',
      unread: 'All unreads',
      thread: 'Threads',
      reaction: 'Mensions & reactions',
      bookmark: 'Saved items',
      search: 'Search',
      master: 'Master maintenance',
      masterGroup: 'Groups',
      masterProject: 'Projects',
      masterPerson: 'Persons',
      masterWorkType: 'Work types',
      analysis: 'Analysis',
      analysisGroup: 'Groups',
      analysisProject: 'Projects',
      analysisPerson: 'Persons',
      analysisWorkType: 'Work types',
      analysisLikeIt: 'Like it',
      analysisReaction: 'Reactions',
    }
  },
  ja: {
    menu: {
      dashborad: 'ダッシュボード',
      unreported: '未提出',
      unread: '全未読',
      thread: 'スレッド',
      reaction: 'メンション＆リアクション',
      bookmark: 'ブックマーク',
      search: '検索',
      master: 'マスタメンテ',
      masterGroup: '部署',
      masterProject: 'プロジェクト',
      masterPerson: '従業員',
      masterWorkType: '業務',
      analysis: '分析',
      analysisGroup: '部署',
      analysisProject: 'プロジェクト',
      analysisPerson: '従業員',
      analysisWorkType: '業務',
      analysisLikeIt: 'いいね',
      analysisReaction: 'リアクション',
    }
  },
};

const i18n = createI18n({
  locale: 'ja',
  fallbackLocale: 'ja',
  messages: messages,
});

export default i18n;