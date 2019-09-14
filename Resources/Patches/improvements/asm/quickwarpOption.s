.thumb
pop	{r3}
cmp	r3,#0
beq	quit

@reload
mov	r0,#2
ldr	r3,=#0x80A690C
mov	lr,r3
.short	0xF800
mov	r0,#0x6A
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
mov	r0,#5
mov	r1,#8
ldr	r3,=#0x804FC90
mov	lr,r3
.short	0xF800
mov	r0,#2
ldr	r3,=#0x8055B8C
mov	lr,r3
.short	0xF800

@count it
ldr	r0,=#0x203FE00
ldrh	r1,[r0,#14]
ldr	r2,=#0xFFFF
cmp	r1,r2
beq	end
add	r1,#1
strh	r1,[r0,#14]
ldr	r2,=#0x2000004
ldrb	r2,[r2]
ldr	r1,=#0x500
mul	r2,r1
ldr	r3,=#0xE001400
add	r2,r3
ldrb	r1,[r0,#15]
ldrb	r0,[r0,#14]
strb	r0,[r2,#14]
strb	r1,[r2,#15]

end:
ldr	r3,=#0x80A5267
bx	r3

quit:
mov	r0,#3
ldr	r3,=#0x80A690C
mov	lr,r3
.short	0xF800
mov	r0,#0x6C
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800
ldr	r3,=#0x80A5267
bx	r3
