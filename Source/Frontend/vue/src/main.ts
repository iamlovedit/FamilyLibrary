import 'uno.css'
import '@/assets/main.css'
import 'vfonts/Lato.css'
import 'vfonts/FiraCode.css'
import { createApp } from 'vue'
import router from '@/router'
import naive from 'naive-ui'
import App from './App.vue'
import { setupStore } from '@/stores'

const app = createApp(App)

setupStore(app)
app.use(router)
app.use(naive)
app.mount('#app')
