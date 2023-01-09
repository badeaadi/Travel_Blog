import '../styles/globals.css'
import type { AppProps } from 'next/app'
import Navigation from '../components/Navigation'
import { SessionProvider } from "next-auth/react"

export default function App({ Component, pageProps: {
  session, ...pageProps
} }: AppProps) {
  return (
    <SessionProvider session={session}>
      <Navigation />
      <Component {...pageProps} />
    </SessionProvider>
  )
}
