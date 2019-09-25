.thumb
push	{lr}
push	{r0-r7}
ldr	r0,=#0x03001190
ldr	r1,[r0]
ldr	r2,=#0x14880000
cmp	r1,r2
bne	end
ldr	r1,=#0x14780000
str	r1,[r0]
end:
pop	{r0-r7}
ldr	r3,=#0x0804CD80
mov	lr,r3
.short	0xF800
mov	r0,#0x6C
ldr	r3,=#0x0804CE85
bx	r3
