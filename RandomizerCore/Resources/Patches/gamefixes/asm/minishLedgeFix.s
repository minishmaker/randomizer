.thumb
mov	r0,#0
strb	r0,[r1,#2]

@ reset push value (the below function call only does it for normal Link)
ldr	r1,=#0x3001160
strb	r0,[r1,#15]

ldr	r3,=#0x8078BFC @ ResetPlayerAnimationAndAction
mov	lr,r3
.short	0xF800
ldr	r3,=#0x8070C7D
bx	r3
