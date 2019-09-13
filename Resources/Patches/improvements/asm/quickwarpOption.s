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
