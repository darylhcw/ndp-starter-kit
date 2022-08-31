import 'normalize.css/normalize.css'
import '../styles/globals.scss'
import type { AppProps } from 'next/app'
import React from 'react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';


export default function MyApp({ Component, pageProps }: AppProps) {
  const [queryClient, _] = React.useState(() => new QueryClient());

  return (
    <QueryClientProvider client={queryClient}>
      <Component {...pageProps} />
      <ReactQueryDevtools initialIsOpen={false}/>
    </QueryClientProvider>
  )
}
