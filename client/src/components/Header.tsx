import { Link, useNavigate } from "@tanstack/react-router";
import { env } from "../env";
import {
	UtensilsCrossed,
	LogIn,
	UserCircle,
	Shield,
	LogOut,
	Menu,
	HouseIcon,
} from "lucide-react";
import { useAuthContext } from "../providers/auth-provider";
import { Button } from "@/components/ui/button";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";

export default function Header() {
	const { isAuthenticated, hasRole, logOut } = useAuthContext();
	const navigate = useNavigate();

	return (
		<header className="border-b">
			<div className="container mx-auto px-4 py-4">
				<nav className="flex items-center justify-between">
					<Link
						to="/"
						className="text-2xl font-bold text-primary flex items-center gap-2"
					>
						<UtensilsCrossed className="h-6 w-6" />
						{env.VITE_APP_TITLE ?? "ForkPoint"}
					</Link>

					{/* Desktop nav */}
					<div className="hidden md:flex items-center gap-6">
						<Link
							to="/restaurants"
							className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
							activeProps={{
								className: "text-sm font-medium text-primary",
							}}
							search={{ page: 1, size: 10 }}
						>
							<HouseIcon className="h-4 w-4" />
							Restaurants
						</Link>
						{!isAuthenticated ? (
							<Button
								className="text-sm font-small transition-colors flex items-center gap-1 hover:text-secondary"
								size="sm"
								asChild
							>
								<Link
									to="/login"
									className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
									activeProps={{
										className:
											"text-sm font-medium text-primary flex items-center gap-1",
									}}
								>
									<LogIn className="h-4 w-4" />
									Get Started
								</Link>
							</Button>
						) : (
							<>
								<Link
									to="/account"
									className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
									activeProps={{
										className:
											"text-sm font-medium text-primary flex items-center gap-1",
									}}
								>
									<UserCircle className="h-4 w-4" />
									Account
								</Link>
								{hasRole("Admin") && (
									<Link
										to="/admin"
										className="text-sm font-medium transition-colors hover:text-primary flex items-center gap-1"
										activeProps={{
											className:
												"text-sm font-medium text-primary flex items-center gap-1",
										}}
									>
										<Shield className="h-4 w-4" />
										Admin
									</Link>
								)}
								<Button
									className="text-sm font-small transition-colors flex items-center gap-1"
									onClick={() => {
										logOut();
										navigate({ to: "/login", replace: true });
									}}
								>
									<LogOut className="h-4 w-4" />
									Sign Out
								</Button>
							</>
						)}
					</div>

					{/* Mobile nav */}
					<div className="md:hidden">
						<Sheet>
							<SheetTrigger asChild>
								<Button variant="outline" size="icon" aria-label="Open menu">
									<Menu className="h-5 w-5" />
								</Button>
							</SheetTrigger>
							<SheetContent side="left">
								<div className="flex items-center gap-2 p-4 border-b">
									<UtensilsCrossed className="h-5 w-5" />
									<span className="font-semibold">
										{env.VITE_APP_TITLE ?? "ForkPoint"}
									</span>
								</div>
								<div className="flex flex-col p-4 gap-3">
									<Link
										to="/restaurants"
										className="text-sm font-medium flex items-center gap-2"
										activeProps={{
											className: "text-primary flex items-center gap-2",
										}}
										search={{ page: 1, size: 10 }}
									>
										<HouseIcon className="h-4 w-4" />
										Restaurants
									</Link>
									{!isAuthenticated ? (
										<Link
											to="/login"
											className="text-sm font-medium flex items-center gap-2"
											activeProps={{
												className: "text-primary flex items-center gap-2",
											}}
										>
											<LogIn className="h-4 w-4" />
											Get Started
										</Link>
									) : (
										<>
											<Link
												to="/account"
												className="text-sm font-medium flex items-center gap-2"
												activeProps={{
													className: "text-primary flex items-center gap-2",
												}}
											>
												<UserCircle className="h-4 w-4" />
												Account
											</Link>
											{hasRole("Admin") && (
												<Link
													to="/admin"
													className="text-sm font-medium flex items-center gap-2"
													activeProps={{
														className: "text-primary flex items-center gap-2",
													}}
												>
													<Shield className="h-4 w-4" />
													Admin
												</Link>
											)}
											<Button
												variant="outline"
												className="justify-start flex items-center gap-2"
												onClick={() => {
													logOut();
												}}
											>
												<LogOut className="h-4 w-4" />
												Sign Out
											</Button>
										</>
									)}
								</div>
							</SheetContent>
						</Sheet>
					</div>
				</nav>
			</div>
		</header>
	);
}
