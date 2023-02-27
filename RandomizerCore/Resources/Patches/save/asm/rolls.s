.thumb
pop	{r3}
@check if queued action is rolling
cmp	r2,#0x18
bne	end
push	{r0-r7}
ldr	r1,offset
ldrh	r2,[r1]
ldr	r3,=#0xFFFF
cmp	r2,r3
beq	endpop
add	r2,#1
strh	r2,[r1]
endpop:
pop	{r0-r7}
end:
ldr	r0,=#0x3001160
mov	r1,#0
strb	r2,[r0,#0x0C]
strb	r1,[r0,#0x0D]
strb	r1,[r3,#0x0C]
push	{r3}
ldr	r3,=#0x807893C
mov	lr,r3
pop	{r3}
.short	0xF800
.align
.ltorg
offset:
