.thumb
push	{r0-r7}
ldr	r1,offset
ldrh	r2,[r1]
add	r2,r0
ldr	r3,=#10000
cmp	r2,r3
blo	store
mov	r2,r3
store:
strh	r2,[r1]
end:
pop	{r0-r7}
push	{lr}
ldr	r3,=#0x2002AE8
ldrh	r1,[r3,#0x18]
add	r2,r1,r0
push	{r3}
ldr	r3,=#0x80522F0
mov	lr,r3
pop	{r3}
.short	0xF800
.align
.ltorg
offset:

