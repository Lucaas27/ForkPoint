import { Link } from '@tanstack/react-router'
import { env } from '../env'
import { UtensilsCrossed, LogIn, UserCircle, Shield } from 'lucide-react'

export default function Header() {
  return (
    <header className="border-b">
      <div className="container mx-auto px-4 py-4">
        <nav className="flex items-center justify-between">
          <Link to="/" className="text-2xl font-bold text-primary flex items-center gap-2">
            <UtensilsCrossed className="h-6 w-6" />
            {env.VITE_APP_TITLE ?? 'ForkPoint'}
          </Link>

          <div className="flex items-center gap-6">
            <Link
              to="/restaurants"
              className="text-sm font-medium transition-colors hover:text-primary"
              activeProps={{
                className: 'text-sm font-medium text-primary',
              }}
            >
              Restaurants
            </Link>

            <Link
              to="/login"
              className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
              activeProps={{
                className: 'text-sm font-medium text-primary flex items-center gap-1',
              }}
            >
              <LogIn className="h-4 w-4" />
              Auth
            </Link>

            <Link
              to="/account"
              className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
              activeProps={{
                className: 'text-sm font-medium text-primary flex items-center gap-1',
              }}
            >
              <UserCircle className="h-4 w-4" />
              Account
            </Link>

            <Link
              to="/admin"
              className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
              activeProps={{
                className: 'text-sm font-medium text-primary flex items-center gap-1',
              }}
            >
              <Shield className="h-4 w-4" />
              Admin
            </Link>
          </div>
        </nav>
      </div>
    </header>
  )
}
