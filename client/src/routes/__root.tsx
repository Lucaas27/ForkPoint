import { Outlet, createRootRouteWithContext } from '@tanstack/react-router'
import Header from '../components/Header'

import type { QueryClient } from '@tanstack/react-query'

interface MyRouterContext {
  queryClient: QueryClient
}

export const Route = createRootRouteWithContext<MyRouterContext>()({
  component: () => (
    <div className="min-h-screen flex flex-col">
      <Header />
      <main className="container mx-auto px-4 py-8 flex-1">
        <Outlet />
      </main>
      <footer className="border-t">
        <div className="container mx-auto px-4 py-6 text-sm text-muted-foreground flex flex-col sm:flex-row items-center justify-between gap-3">
          <div>
            © {new Date().getFullYear()} ForkPoint. All rights reserved.
          </div>
          <nav className="flex items-center gap-4">
            <a href="/restaurants" className="hover:underline">Restaurants</a>
            <a href="/account" className="hover:underline">Account</a>
            <a href="/admin" className="hover:underline">Admin</a>
          </nav>
        </div>
      </footer>
      {/* <TanStackDevtools
        config={{
          position: 'bottom-right',
        }}
        plugins={[
          {
            name: 'Tanstack Router',
            render: <TanStackRouterDevtoolsPanel />,
          },
          TanStackQueryDevtools,
        ]}
      /> */}
    </div>
  ),
})
