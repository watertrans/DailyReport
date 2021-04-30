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
    },
    dialog: {
      unauthorized: 'タイムアウト',
      unauthorizedMessage: 'アクセストークンがタイムアウトしました。認証画面に戻りますがよろしいですか？',
    },
    dashboard: {
      unreadTitle: '未読日報',
      unreadDetail: '所属部署の日報の未読件数',
      unreportedTitle: '未提出日報',
      unreportedDetail: '所属部署の日報の未提出件数',
      workTypePieTitle: '今月の業務',
      workTypeLineTitle: '過去の業務',
      rankingTitle: 'ランキング',
      rankingTabReceivedTitle: 'いいね獲得数',
      rankingTabSentTitle: 'いいね送信数',
    },
  },
};

const i18n = createI18n({
  locale: 'ja',
  fallbackLocale: 'ja',
  messages: messages,
});

export default i18n;