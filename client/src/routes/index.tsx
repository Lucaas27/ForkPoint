import { createFileRoute, Link } from "@tanstack/react-router";
import { Button } from "../components/ui/button";
import {
	Utensils,
	LayoutGrid,
	ShieldCheck,
	Sparkles,
	UserPlus,
	Store,
	ListChecks,
} from "lucide-react";
import { env } from "../env";
import { useAuthContext } from "@/features/auth/AuthProvider";

export const Route = createFileRoute("/")({
	component: Landing,
});

function Landing() {
	const title = env.VITE_APP_TITLE || "ForkPoint";
	const { isAuthenticated } = useAuthContext();
	return (
		<>
			<div className="relative isolate overflow-hidden rounded-xl border bg-linear-to-b from-background to-muted">
				<div className="px-6 py-16 md:px-12 md:py-24 lg:px-16">
					<div className="mx-auto max-w-3xl text-center">
						<div className="flex justify-center">
							<img src="/logo.svg" alt="ForkPoint logo" className="h-16 w-16" />
						</div>
						<h1 className="text-4xl md:text-5xl font-bold tracking-tight">
							{title}
						</h1>
						<p className="mt-4 text-lg text-muted-foreground">
							Browse restaurants. Create and manage your own.
						</p>
						<div className="mt-8 flex flex-col sm:flex-row gap-3 justify-center">
							<Link to="/restaurants" search={{ page: 1, size: 10 }}>
								<Button size="lg">Browse Restaurants</Button>
							</Link>
							{!isAuthenticated && (
								<Link to="/login">
									<Button variant="outline" size="lg">
										Sign In
									</Button>
								</Link>
							)}
						</div>
					</div>
				</div>
			</div>

			<section className="mt-12">
				<div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
					<div className="rounded-lg border p-5">
						<div className="flex items-center gap-3">
							<div className="rounded-md bg-primary/10 p-2 text-primary">
								<Utensils className="h-5 w-5" />
							</div>
							<h3 className="font-semibold">Browse Restaurants</h3>
						</div>
						<p className="mt-2 text-sm text-muted-foreground">
							Find places quickly with a clean, simple UI.
						</p>
					</div>
					<div className="rounded-lg border p-5">
						<div className="flex items-center gap-3">
							<div className="rounded-md bg-primary/10 p-2 text-primary">
								<LayoutGrid className="h-5 w-5" />
							</div>
							<h3 className="font-semibold">Create & Manage</h3>
						</div>
						<p className="mt-2 text-sm text-muted-foreground">
							Add your restaurant and edit menus.
						</p>
					</div>
					<div className="rounded-lg border p-5">
						<div className="flex items-center gap-3">
							<div className="rounded-md bg-primary/10 p-2 text-primary">
								<ShieldCheck className="h-5 w-5" />
							</div>
							<h3 className="font-semibold">Roles & Access</h3>
						</div>
						<p className="mt-2 text-sm text-muted-foreground">
							Admins can assign roles and control access.
						</p>
					</div>
					<div className="rounded-lg border p-5">
						<div className="flex items-center gap-3">
							<div className="rounded-md bg-primary/10 p-2 text-primary">
								<Sparkles className="h-5 w-5" />
							</div>
							<h3 className="font-semibold">Modern UI</h3>
						</div>
						<p className="mt-2 text-sm text-muted-foreground">
							React 19, TanStack Router/Query, Tailwind v4, shadcn/ui.
						</p>
					</div>
				</div>
			</section>

			<section className="mt-12">
				<h2 className="text-center text-xl font-semibold">How it works</h2>
				<div className="mt-6 grid gap-6 sm:grid-cols-3">
					<div className="rounded-lg border p-6 text-center">
						<div className="mx-auto mb-3 flex size-10 items-center justify-center rounded-full bg-primary/10 text-primary font-medium">
							1
						</div>
						<div className="mx-auto mb-2 flex size-10 items-center justify-center rounded-md bg-primary/10 text-primary">
							<UserPlus className="h-5 w-5" />
						</div>
						<h3 className="font-semibold">Sign up or Login</h3>
						<p className="mt-1 text-sm text-muted-foreground">
							Create an account or sign in to get started.
						</p>
					</div>
					<div className="rounded-lg border p-6 text-center">
						<div className="mx-auto mb-3 flex size-10 items-center justify-center rounded-full bg-primary/10 text-primary font-medium">
							2
						</div>
						<div className="mx-auto mb-2 flex size-10 items-center justify-center rounded-md bg-primary/10 text-primary">
							<Store className="h-5 w-5" />
						</div>
						<h3 className="font-semibold">Create your restaurant</h3>
						<p className="mt-1 text-sm text-muted-foreground">
							Add details like name, description, and category.
						</p>
					</div>
					<div className="rounded-lg border p-6 text-center">
						<div className="mx-auto mb-3 flex size-10 items-center justify-center rounded-full bg-primary/10 text-primary font-medium">
							3
						</div>
						<div className="mx-auto mb-2 flex size-10 items-center justify-center rounded-md bg-primary/10 text-primary">
							<ListChecks className="h-5 w-5" />
						</div>
						<h3 className="font-semibold">Add your menu</h3>
						<p className="mt-1 text-sm text-muted-foreground">
							Create menu items.
						</p>
					</div>
				</div>
			</section>
		</>
	);
}
