.thumb
push	{r4-r5,lr}
mov	r5,r0
mov	r0,r1
mov	r1,r2
ldr	r3,=#0x804A77C
mov	lr,r3
.short	0xF800
mov	r4,r0
cmp	r4,#0
beq	end
str	r5,[r4,#0x50]
mov	r0,r5
mov	r1,r4
ldr	r3,=#0x806F464
mov	lr,r3
.short	0xF800
mov	r0,r4
end:
pop	{r4-r5,pc}
