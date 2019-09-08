.thumb
push	{lr}
ldr	r0,=#0x95
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
ldr	r0,=#0x2002B00
ldr	r1,[r0]
ldr	r2,=#200
cmp	r2,r1
blo	np
mov	r2,r1
np:
sub	r1,r2
str	r1,[r0]
pop	{pc}
