import { createFileRoute, useNavigate } from '@tanstack/react-router'
import { useLogin, useRegister, useLogout } from '../../features/auth/mutations'
import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Input } from '../../components/ui/input'
import { Label } from '../../components/ui/label'
import { Button } from '../../components/ui/button'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../components/ui/tabs'
import { Separator } from '../../components/ui/separator'
import { LogIn, UserPlus, LogOut } from 'lucide-react'
import { toast } from 'sonner'

export const Route = createFileRoute('/login/')({
    component: Login,
})

function Login() {
    const navigate = useNavigate()
    const [email, setEmail] = useState('forkpointuser@gmail.com')
    const [password, setPassword] = useState('UserPassword1!')
    const mLogin = useLogin()
    const mRegister = useRegister()
    const mLogout = useLogout()

    return (
        <div className="max-w-md mx-auto">
            <Card>
                <CardHeader>
                    <CardTitle className="text-2xl">Authentication</CardTitle>
                    <CardDescription>Login or create a new account</CardDescription>
                </CardHeader>
                <CardContent>
                    <Tabs defaultValue="login" className="w-full">
                        <TabsList className="grid w-full grid-cols-2">
                            <TabsTrigger value="login">Login</TabsTrigger>
                            <TabsTrigger value="register">Register</TabsTrigger>
                        </TabsList>

                        <TabsContent value="login" className="space-y-4">
                            <div className="space-y-2">
                                <Label htmlFor="login-email">Email</Label>
                                <Input
                                    id="login-email"
                                    type="email"
                                    placeholder="Enter your email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="login-password">Password</Label>
                                <Input
                                    id="login-password"
                                    type="password"
                                    placeholder="Enter your password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>

                            <Button
                                className="w-full"
                                onClick={() =>
                                    mLogin.mutate(
                                        { email, password },
                                        {
                                            onSuccess: () => {
                                                toast.success('Logged in')
                                                navigate({ to: '/restaurants' })
                                            },
                                            onError: (err: any) => {
                                                toast.error(err?.message || 'Login failed')
                                            },
                                        },
                                    )
                                }
                                disabled={mLogin.isPending}
                            >
                                <LogIn className="h-4 w-4 mr-2" />
                                {mLogin.isPending ? 'Logging in...' : 'Login'}
                            </Button>
                        </TabsContent>

                        <TabsContent value="register" className="space-y-4">
                            <div className="space-y-2">
                                <Label htmlFor="register-email">Email</Label>
                                <Input
                                    id="register-email"
                                    type="email"
                                    placeholder="Enter your email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="register-password">Password</Label>
                                <Input
                                    id="register-password"
                                    type="password"
                                    placeholder="Create a password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>

                            <Button
                                className="w-full"
                                onClick={() =>
                                    mRegister.mutate(
                                        { email, password },
                                        {
                                            onSuccess: (data: any) => {
                                                const msg = data?.message || 'Account created'
                                                console.log('register success:', data)
                                                toast.success(msg)
                                            },
                                            onError: (err: any) => toast.error(err?.message || 'Registration failed'),
                                        },
                                    )
                                }
                                disabled={mRegister.isPending}
                            >
                                <UserPlus className="h-4 w-4 mr-2" />
                                {mRegister.isPending ? 'Creating account...' : 'Register'}
                            </Button>
                        </TabsContent>
                    </Tabs>

                    <Separator className="my-6" />

                    <Button
                        variant="outline"
                        className="w-full"
                        onClick={() =>
                            mLogout.mutate(undefined, {
                                onSuccess: () => toast.success('Logged out'),
                                onError: (err: any) => toast.error(err?.message || 'Logout failed'),
                            })
                        }
                        disabled={mLogout.isPending}
                    >
                        <LogOut className="h-4 w-4 mr-2" />
                        Logout
                    </Button>
                </CardContent>
            </Card>
        </div>
    )
}
