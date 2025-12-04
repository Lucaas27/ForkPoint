Welcome to your new TanStack app!

# Getting Started

To run this application:

```bash
npm install
npm run start
```

# Building For Production

To build this application for production:

```bash
npm run build
```

## Testing

This project uses [Vitest](https://vitest.dev/) for testing. You can run the tests with:

```bash
npm run test
```

## Styling

This project uses [Tailwind CSS](https://tailwindcss.com/) for styling.


## Linting & Formatting

This project uses [Biome](https://biomejs.dev/) for linting and formatting. The following scripts are available:


```bash
npm run lint
npm run format
npm run check
```
# ForkPoint Client

Simple React 19 client scaffolded with `create-tsrouter-app` to showcase the ForkPoint API. Uses TanStack Router (file-based), React Query for data fetching, Axios, and Tailwind CSS v4.

## Quick Start

```bash
cd client
npm install
echo VITE_API_BASE_URL=http://localhost:8080 > .env
npm run dev
```

Open `http://localhost:3000`.

## Screenshots

- Home: `docs/screenshots/home.png`
- Restaurants: `docs/screenshots/restaurants.png`
- Restaurant Detail: `docs/screenshots/restaurant-detail.png`
- Auth: `docs/screenshots/auth.png`
- Account: `docs/screenshots/account.png`
- Admin: `docs/screenshots/admin.png`

Create the `client/docs/screenshots/` images if you want screenshots in the README, or ignore these lines.

## Endpoint Mapping

- Restaurants
  - `GET /api/restaurants` → List: `src/routes/restaurants.index.tsx`
  - `GET /api/restaurants/{id}` → Detail: `src/routes/restaurants.$id.tsx`
  - `POST /api/restaurants/create` → Create: `src/routes/restaurants.create.tsx`
  - `PATCH /api/restaurants/{id}` → Update: helper in `src/api/restaurants.ts`
  - `DELETE /api/restaurants/{id}` → Delete: `src/routes/restaurants.$id.tsx`

- Menu Items
  - `GET /api/restaurant/{restaurantId}/menu-items` → List: `src/routes/restaurants.$id.tsx`
  - `GET /api/restaurant/{restaurantId}/menu-items/{menuItemId}` → Detail: helper in `src/api/menuItems.ts`
  - `POST /api/restaurant/{restaurantId}/menu-items/create` → Create: helper in `src/api/menuItems.ts`
  - `DELETE /api/restaurant/{restaurantId}/menu-items/{menuItemId}` → Delete: helper in `src/api/menuItems.ts`
  - `DELETE /api/restaurant/{restaurantId}/menu-items` → Delete All: `src/routes/restaurants.$id.tsx`

- Auth
  - `POST /api/auth/register` → Register: `src/routes/login.tsx`
  - `POST /api/auth/login` → Login: `src/routes/login.tsx` (stores `Bearer` in Axios)
  - `POST /api/auth/logout` → Logout: `src/routes/login.tsx`
  - `POST /api/auth/refresh-token` → Refresh: helper in `src/api/auth.ts`
  - `GET /api/auth/google-login` → External login: not showcased in UI

- Account
  - `PATCH /api/account/update` → Update me: `src/routes/account.tsx`
  - `PATCH /api/account/update/{userId}` → Admin update: helper in `src/api/account.ts`
  - `POST /api/account/forgot-password` → Forgot: helper in `src/api/account.ts`
  - `POST /api/account/reset-password` → Reset: helper in `src/api/account.ts`
  - `GET /api/account/verify` → Confirm email: helper in `src/api/account.ts`
  - `POST /api/account/resend-email-confirmation` → Resend: helper in `src/api/account.ts`
  - `GET /api/auth/restaurants` → My restaurants: `src/routes/account.tsx`

- Admin
  - `POST /api/admin/users/roles` → Assign role: `src/routes/admin.tsx`
  - `DELETE /api/admin/users/roles` → Remove role: `src/routes/admin.tsx`

## Tech Notes

- Routing: File-based routes under `src/routes/` from create-tsrouter-app.
- Data Fetching: React Query context is provided by the scaffold (`src/integrations/tanstack-query`).
- HTTP: Axios instance in `src/api/client.ts` uses `VITE_API_BASE_URL`. Token is attached as `Authorization: Bearer ...`.
- Styling: Tailwind v4; minimal utility classes. You can add shadcn components if desired.

## Troubleshooting

- 401/403 responses: Ensure you login via the Auth page before calling protected endpoints.
- API base URL: Update `.env` if your API runs on a different port or HTTPS (`https://localhost:7078`).
