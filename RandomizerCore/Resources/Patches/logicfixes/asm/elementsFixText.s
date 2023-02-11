.thumb
ldr	r3,=#0x8053E34
mov	lr,r3
.short	0xF800
ldr	r0,=#0x3001250
ldrh	r0,[r0]
mov	r1,r4
ldr	r3,=#0x8055F18
mov	lr,r3
.short	0xF800
pop	{r4,pc}
